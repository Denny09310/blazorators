﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.JSInterop;

internal sealed class DefaultSpeechRecognitionService(IJSInProcessRuntime javaScript)
    : ISpeechRecognitionService
{
    readonly SpeechRecognitionCallbackRegistry _callbackRegistry = new();

    IJSInProcessObjectReference? _speechRecognitionModule;
    SpeechRecognitionSubject? _speechRecognition;

    void InitializeSpeechRecognitionSubject()
    {
        if (_speechRecognition is not null)
        {
            CancelSpeechRecognition(false);
            _speechRecognition.Dispose();
        }

        _speechRecognition = SpeechRecognitionSubject.Factory(
            _callbackRegistry.InvokeOnRecognized);
    }

    /// <inheritdoc />
    public async Task InitializeModuleAsync(bool logModuleDetails)
    {
        _speechRecognitionModule =
            await javaScript.InvokeAsync<IJSInProcessObjectReference>(
                "import",
                "./_content/Blazor.SpeechRecognition.WebAssembly/blazorators.speechRecognition.js");

        if (logModuleDetails)
        {
            _speechRecognitionModule?.InvokeVoid(
                JavaScriptInteropMethodIdentifiers.LogModuleDetails);
        }
    }

    /// <inheritdoc />
    public void CancelSpeechRecognition(
        bool isAborted) =>
        _speechRecognitionModule?.InvokeVoid(
            JavaScriptInteropMethodIdentifiers.CancelSpeechRecognition,
            isAborted);

    /// <inheritdoc />
    public IDisposable RecognizeSpeech(
        string language,
        Action<string> onRecognized,
        Action<SpeechRecognitionErrorEvent>? onError,
        Action? onStarted,
        Action? onEnded)
    {
        InitializeSpeechRecognitionSubject();

        var key = Guid.NewGuid();
        _callbackRegistry.RegisterOnRecognized(key, onRecognized);

        if (onError is not null)
            _callbackRegistry.RegisterOnError(key, onError);
        if (onStarted is not null)
            _callbackRegistry.RegisterOnStarted(key, onStarted);
        if (onEnded is not null)
            _callbackRegistry.RegisterOnEnded(key, onEnded);

        _speechRecognitionModule?.InvokeVoid(
            JavaScriptInteropMethodIdentifiers.RecognizeSpeech,
            DotNetObjectReference.Create(this),
            language,
            key,
            nameof(OnSpeechRecognized),
            nameof(OnRecognitionError),
            nameof(OnStarted),
            nameof(OnEnded));

        return _speechRecognition!;
    }

    [JSInvokable]
    public void OnStarted(string key) => _callbackRegistry.InvokeOnStarted(key);

    [JSInvokable]
    public void OnEnded(string key) => _callbackRegistry.InvokeOnEnded(key);

    [JSInvokable]
    public void OnRecognitionError(string key, SpeechRecognitionErrorEvent errorEvent) =>
        _callbackRegistry.InvokeOnError(key, errorEvent);

    [JSInvokable]
    public void OnSpeechRecognized(string key, string transcript, bool isFinal) =>
        _speechRecognition?.RecognitionReceived(
            new SpeechRecognitionResult(key, transcript, isFinal));

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        _speechRecognition?.Dispose();
        _speechRecognition = null;

        if (_speechRecognitionModule is not null)
        {
            await _speechRecognitionModule.DisposeAsync();
            _speechRecognitionModule = null;
        }
    }
}
