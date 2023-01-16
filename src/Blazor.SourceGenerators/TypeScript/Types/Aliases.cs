﻿// Copyright (c) David Pine. All rights reserved.
// Licensed under the MIT License.

#nullable disable
using Blazor.SourceGenerators.TypeScript.Compiler;

namespace Blazor.SourceGenerators.TypeScript.Types;

public sealed class OpenBracketToken : Token
{
}

public sealed class DotDotDotToken : Token
{
    public DotDotDotToken() => Kind = TypeScriptSyntaxKind.DotDotDotToken;
}

public sealed class DotToken : Token
{
    public DotToken() => Kind = TypeScriptSyntaxKind.DotToken;
}

public sealed class QuestionToken : Token
{
    public QuestionToken() => Kind = TypeScriptSyntaxKind.QuestionToken;
}

public sealed class ColonToken : Token
{
    public ColonToken() => Kind = TypeScriptSyntaxKind.ColonToken;
}

public sealed class EqualsToken : Token
{
    public EqualsToken() => Kind = TypeScriptSyntaxKind.EqualsToken;
}

public sealed class AsteriskToken : Token
{
    public AsteriskToken() => Kind = TypeScriptSyntaxKind.AsteriskToken;
}

public sealed class EqualsGreaterThanToken : Token
{
    public EqualsGreaterThanToken() => Kind = TypeScriptSyntaxKind.EqualsGreaterThanToken;
}

public sealed class EndOfFileToken : Token
{
    public EndOfFileToken() => Kind = TypeScriptSyntaxKind.EndOfFileToken;
}

public sealed class AtToken : Token
{
    public AtToken() => Kind = TypeScriptSyntaxKind.AtToken;
}

public sealed class ReadonlyToken : Token
{
    public ReadonlyToken() => Kind = TypeScriptSyntaxKind.ReadonlyKeyword;
}

public sealed class AwaitKeywordToken : Token
{
    public AwaitKeywordToken() => Kind = TypeScriptSyntaxKind.AwaitKeyword;
}

public sealed class Modifier : Node
{
}

public sealed class ModifiersArray : NodeArray<Modifier>
{
}

public interface IEntityName : INode
{
}

public interface IPropertyName : INode
{
}

public interface IDeclarationName : INode
{
}

public interface IBindingName : INode
{
}

public interface IObjectLiteralElementLike : INode
{
}

public interface IBindingPattern : INode
{
    NodeArray<IArrayBindingElement> Elements { get; set; }
}

public interface IArrayBindingElement : INode
{
}

public interface IAccessorDeclaration : ISignatureDeclaration, IClassElement, IObjectLiteralElementLike
{
    IBlockOrExpression Body { get; set; }
}

public interface IFunctionOrConstructorTypeNode : ISignatureDeclaration, ITypeNode
{
}

public interface IUnionOrIntersectionTypeNode : ITypeNode
{
    NodeArray<ITypeNode> Types { get; set; }
}

public sealed class AssignmentOperatorToken : Token
{
}

public interface IDestructuringAssignment : INode
{
}

public interface IBindingOrAssignmentElement : INode
{
}

public interface IBindingOrAssignmentElementRestIndicator : INode
{
}

public interface IBindingOrAssignmentElementTarget : INode
{
}

public interface IObjectBindingOrAssignmentPattern : INode
{
}

public interface IArrayBindingOrAssignmentPattern : INode
{
}

public interface IAssignmentPattern : INode
{
}

public interface IBindingOrAssignmentPattern : INode
{
}

public sealed class FunctionBody : Block
{
}

public interface IConciseBody : INode
{
}

public interface ITemplateLiteral : INode
{
}

public interface IEntityNameExpression : INode
{
}

public interface IEntityNameOrEntityNameExpression : INode
{
}

public interface ISuperProperty : INode
{
}

public interface ICallLikeExpression : INode
{
}

public interface IAssertionExpression : IExpression
{
}

public interface IJsxOpeningLikeElement : IExpression
{
}

public interface IJsxAttributeLike : IObjectLiteralElement
{
}

public interface IJsxTagNameExpression : IExpression
{
}

public interface IJsxChild : INode
{
}

public interface IBlockLike : INode
{
}

public interface IForInitializer : INode
{
}

public interface IBreakOrContinueStatement : IStatement
{
    Identifier Label { get; set; }
}

public interface ICaseOrDefaultClause : INode
{
}

public interface IDeclarationWithTypeParameters : INode
{
}

public interface IModuleName : INode
{
}

public interface IModuleBody : INode
{
}

public interface INamespaceBody : INode
{
}

public interface IJsDocNamespaceBody : INode
{
}

public interface IModuleReference : INode
{
}

public interface INamedImportBindings : INode, INamedImportsOrExports
{
}

public interface INamedImportsOrExports : INode
{
}

public interface IImportOrExportSpecifier : IDeclaration
{
    Identifier PropertyName { get; set; }
}

public interface IJsDocTypeReferencingNode : IJsDocType
{
}

public sealed class FlowType : object
{
}

public sealed class ITypePredicate : TypePredicateBase
{
}

public interface IAnyImportSyntax : INode
{
}

public sealed class SymbolTable : Map
{
}

public interface IDestructuringPattern : INode
{
}

public interface IBaseType : IType
{
}

public interface IStructuredType : IType
{
}

public sealed class CompilerOptionsValue : object
{
}

public sealed class CommandLineOption : CommandLineOptionBase
{
}

public sealed class TransformerFactory<T>
{
}

public sealed class Transformer
{
}

public sealed class Visitor
{
}

public sealed class VisitResult : object
{
}

public sealed class MapLike
{
}

public class Map
{
    public int Size { get; set; }
}

public sealed class Iterator
{
}

public sealed class FileMap
{
}

public interface ITextRange
{
    int? Pos { get; set; }
    int? End { get; set; }
}

public class TextRange : ITextRange
{
    public int? Pos { get; set; }
    public int? End { get; set; }
}

public class NodeArray<T> : List<T>, ITextRange
{
    public NodeArray()
    {
    }

    public NodeArray(T[] elements)
        : base(elements.ToList())
    {
    }

    public bool HasTrailingComma { get; set; }
    public TransformFlags TransformFlags { get; set; }
    public int? Pos { get; set; }
    public int? End { get; set; }
}

public class Token : Node
{
}

public class Identifier : PrimaryExpression, IJsxTagNameExpression, IEntityName, IPropertyName
{
    public Identifier() => Kind = TypeScriptSyntaxKind.Identifier;

    public string Text { get; set; }
    public TypeScriptSyntaxKind OriginalKeywordKind { get; set; }
    public GeneratedIdentifierKind AutoGenerateKind { get; set; }
    public int AutoGenerateId { get; set; }
    public bool IsInJsDocNamespace { get; set; }
}

public sealed class TransientIdentifier : Identifier
{
    public Symbol ResolvedSymbol { get; set; }
}

public sealed class GeneratedIdentifier : Identifier
{
}

public sealed class QualifiedName : Node, IEntityName
{
    public QualifiedName() => Kind = TypeScriptSyntaxKind.QualifiedName;

    public IEntityName Left { get; set; }
    public Identifier Right { get; set; }
}

public interface IDeclaration : INode
{
    object DeclarationBrand { get; set; }
    INode Name { get; set; }
}

public class Declaration : Node, IDeclaration
{
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
}

public interface IDeclarationStatement : INode, IDeclaration, IStatement
{
    // Node Name { get; set; } // Identifier | StringLiteral | NumericLiteral
}

public class DeclarationStatement : Node, IDeclarationStatement, IDeclaration, IStatement
{
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object StatementBrand { get; set; }
}

public sealed class ComputedPropertyName : Node, IPropertyName
{
    public ComputedPropertyName() => Kind = TypeScriptSyntaxKind.ComputedPropertyName;

    public IExpression Expression { get; set; }
}

public sealed class Decorator : Node
{
    public Decorator() => Kind = TypeScriptSyntaxKind.Decorator;

    public IExpression Expression { get; set; }
}

public sealed class TypeParameterDeclaration : Declaration
{
    public TypeParameterDeclaration() => Kind = TypeScriptSyntaxKind.TypeParameter;

    public ITypeNode Constraint { get; set; }
    public ITypeNode Default { get; set; }
    public IExpression Expression { get; set; }
}

public interface ISignatureDeclaration : IDeclaration
{
    NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    NodeArray<ParameterDeclaration> Parameters { get; set; }
    ITypeNode Type { get; set; }
}

public class SignatureDeclaration : Declaration, ISignatureDeclaration
{
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class CallSignatureDeclaration : Declaration, ISignatureDeclaration, ITypeElement
{
    public CallSignatureDeclaration() => Kind = TypeScriptSyntaxKind.CallSignature;

    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public sealed class ConstructSignatureDeclaration : Declaration, ISignatureDeclaration, ITypeElement
{
    public ConstructSignatureDeclaration() => Kind = TypeScriptSyntaxKind.ConstructSignature;

    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public sealed class VariableDeclaration : Declaration, IVariableLikeDeclaration
{
    public VariableDeclaration() => Kind = TypeScriptSyntaxKind.VariableDeclaration;

    public ITypeNode Type { get; set; }
    public IExpression Initializer { get; set; }
    public IPropertyName PropertyName { get; set; }
    public DotDotDotToken DotDotDotToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public interface IVariableDeclarationListOrExpression : INode
{
}

public interface IVariableDeclarationList : INode, IVariableDeclarationListOrExpression
{
    NodeArray<VariableDeclaration> Declarations { get; set; }
}

public sealed class VariableDeclarationList : Node, IVariableDeclarationList
{
    public VariableDeclarationList() => Kind = TypeScriptSyntaxKind.VariableDeclarationList;

    public NodeArray<VariableDeclaration> Declarations { get; set; }
}

public sealed class ParameterDeclaration : Declaration, IVariableLikeDeclaration
{
    public ParameterDeclaration() => Kind = TypeScriptSyntaxKind.Parameter;

    public DotDotDotToken DotDotDotToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public ITypeNode Type { get; set; }
    public IExpression Initializer { get; set; }
    public IPropertyName PropertyName { get; set; }
}

public sealed class BindingElement : Declaration, IArrayBindingElement, IVariableLikeDeclaration
{
    public BindingElement() => Kind = TypeScriptSyntaxKind.BindingElement;

    public IPropertyName PropertyName { get; set; }
    public DotDotDotToken DotDotDotToken { get; set; }
    public IExpression Initializer { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public ITypeNode Type { get; set; }
}

public class PropertySignature : TypeElement, IVariableLikeDeclaration
{
    public PropertySignature() => Kind = TypeScriptSyntaxKind.PropertySignature;

    public ITypeNode Type { get; set; }
    public IExpression Initializer { get; set; }
    public IPropertyName PropertyName { get; set; }
    public DotDotDotToken DotDotDotToken { get; set; }
}

public sealed class PropertyDeclaration : ClassElement, IVariableLikeDeclaration
{
    public PropertyDeclaration() => Kind = TypeScriptSyntaxKind.PropertyDeclaration;

    public QuestionToken QuestionToken { get; set; }
    public ITypeNode Type { get; set; }
    public IExpression Initializer { get; set; }
    public IPropertyName PropertyName { get; set; }
    public DotDotDotToken DotDotDotToken { get; set; }
}

public interface IObjectLiteralElement : IDeclaration
{
    object ObjectLiteralBrandBrand { get; set; }
}

public class ObjectLiteralElement : Declaration, IObjectLiteralElement
{
    public object ObjectLiteralBrandBrand { get; set; }
}

public sealed class PropertyAssignment : ObjectLiteralElement, IObjectLiteralElementLike, IVariableLikeDeclaration
{
    public PropertyAssignment() => Kind = TypeScriptSyntaxKind.PropertyAssignment;

    public QuestionToken QuestionToken { get; set; }
    public IExpression Initializer { get; set; }
    public IPropertyName PropertyName { get; set; }
    public DotDotDotToken DotDotDotToken { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class ShorthandPropertyAssignment : ObjectLiteralElement, IObjectLiteralElementLike
{
    public ShorthandPropertyAssignment() => Kind = TypeScriptSyntaxKind.ShorthandPropertyAssignment;

    public QuestionToken QuestionToken { get; set; }
    public Token EqualsToken { get; set; }
    public IExpression ObjectAssignmentInitializer { get; set; }
}

public sealed class SpreadAssignment : ObjectLiteralElement, IObjectLiteralElementLike
{
    public SpreadAssignment() => Kind = TypeScriptSyntaxKind.SpreadAssignment;

    public IExpression Expression { get; set; }
}

public interface IVariableLikeDeclaration : IDeclaration
{
    IPropertyName PropertyName { get; set; }
    DotDotDotToken DotDotDotToken { get; set; }
    QuestionToken QuestionToken { get; set; }
    ITypeNode Type { get; set; }
    IExpression Initializer { get; set; }
}

public sealed class PropertyLikeDeclaration : Declaration
{
}

public sealed class ObjectBindingPattern : Node, IBindingPattern
{
    public ObjectBindingPattern() => Kind = TypeScriptSyntaxKind.ObjectBindingPattern;

    public NodeArray<IArrayBindingElement> Elements { get; set; }
}

public sealed class ArrayBindingPattern : Node, IBindingPattern
{
    public ArrayBindingPattern() => Kind = TypeScriptSyntaxKind.ArrayBindingPattern;

    public NodeArray<IArrayBindingElement> Elements { get; set; }
}

public interface IFunctionLikeDeclaration : ISignatureDeclaration
{
    object FunctionLikeDeclarationBrand { get; set; }
    AsteriskToken AsteriskToken { get; set; }
    QuestionToken QuestionToken { get; set; }
    IBlockOrExpression Body { get; set; }
}

public sealed class FunctionLikeDeclaration : SignatureDeclaration, IFunctionLikeDeclaration
{
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
}

public sealed class FunctionDeclaration : Node, IFunctionLikeDeclaration, IDeclarationStatement
{
    public FunctionDeclaration() => Kind = TypeScriptSyntaxKind.FunctionDeclaration;

    public object StatementBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
}

public sealed class MethodSignature : Declaration, ISignatureDeclaration, ITypeElement, IFunctionLikeDeclaration
{
    public MethodSignature() => Kind = TypeScriptSyntaxKind.MethodSignature;

    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public sealed class MethodDeclaration : Declaration, IFunctionLikeDeclaration, IClassElement, IObjectLiteralElement,
    IObjectLiteralElementLike
{
    public MethodDeclaration() => Kind = TypeScriptSyntaxKind.MethodDeclaration;

    public object ClassElementBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object ObjectLiteralBrandBrand { get; set; }
}

public sealed class ConstructorDeclaration : Declaration, IFunctionLikeDeclaration, IClassElement
{
    public ConstructorDeclaration() => Kind = TypeScriptSyntaxKind.Constructor;

    public object ClassElementBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class SemicolonClassElement : ClassElement
{
    public SemicolonClassElement() => Kind = TypeScriptSyntaxKind.SemicolonClassElement;
}

public sealed class GetAccessorDeclaration : Declaration, IFunctionLikeDeclaration, IClassElement, IObjectLiteralElement,
    IAccessorDeclaration
{
    public GetAccessorDeclaration() => Kind = TypeScriptSyntaxKind.GetAccessor;

    public object ClassElementBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object ObjectLiteralBrandBrand { get; set; }
}

public sealed class SetAccessorDeclaration : Declaration, IFunctionLikeDeclaration, IClassElement, IObjectLiteralElement,
    IAccessorDeclaration
{
    public SetAccessorDeclaration() => Kind = TypeScriptSyntaxKind.SetAccessor;

    public object ClassElementBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object ObjectLiteralBrandBrand { get; set; }
}

public sealed class IndexSignatureDeclaration : Declaration, ISignatureDeclaration, IClassElement, ITypeElement
{
    public IndexSignatureDeclaration() => Kind = TypeScriptSyntaxKind.IndexSignature;

    public object ClassElementBrand { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public interface ITypeNode : INode
{
    object TypeNodeBrand { get; set; }
}

public class TypeNode : Node, ITypeNode
{
    public object TypeNodeBrand { get; set; }
}

public interface IKeywordTypeNode : ITypeNode
{
}

public sealed class KeywordTypeNode : TypeNode, IKeywordTypeNode
{
}

public sealed class ThisTypeNode : TypeNode
{
    public ThisTypeNode() => Kind = TypeScriptSyntaxKind.ThisType;
}

public sealed class FunctionTypeNode : Node, ITypeNode, IFunctionOrConstructorTypeNode
{
    public FunctionTypeNode() => Kind = TypeScriptSyntaxKind.FunctionType;

    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class ConstructorTypeNode : Node, ITypeNode, IFunctionOrConstructorTypeNode
{
    public ConstructorTypeNode() => Kind = TypeScriptSyntaxKind.ConstructorType;

    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class TypeReferenceNode : TypeNode
{
    public TypeReferenceNode() => Kind = TypeScriptSyntaxKind.TypeReference;

    public IEntityName TypeName { get; set; }
    public NodeArray<ITypeNode> TypeArguments { get; set; }
}

public sealed class TypePredicateNode : TypeNode
{
    public TypePredicateNode() => Kind = TypeScriptSyntaxKind.TypePredicate;

    public Node ParameterName { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class TypeQueryNode : TypeNode
{
    public TypeQueryNode() => Kind = TypeScriptSyntaxKind.TypeQuery;

    public IEntityName ExprName { get; set; }
}

public sealed class TypeLiteralNode : Node, ITypeNode, IDeclaration
{
    public TypeLiteralNode() => Kind = TypeScriptSyntaxKind.TypeLiteral;

    public NodeArray<ITypeElement> Members { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class ArrayTypeNode : TypeNode
{
    public ArrayTypeNode() => Kind = TypeScriptSyntaxKind.ArrayType;

    public ITypeNode ElementType { get; set; }
}

public sealed class TupleTypeNode : TypeNode
{
    public TupleTypeNode() => Kind = TypeScriptSyntaxKind.TupleType;

    public NodeArray<ITypeNode> ElementTypes { get; set; }
}

public sealed class UnionTypeNode : TypeNode, IUnionOrIntersectionTypeNode
{
    public UnionTypeNode() => Kind = TypeScriptSyntaxKind.UnionType;

    public NodeArray<ITypeNode> Types { get; set; }
}

public sealed class IntersectionTypeNode : TypeNode, IUnionOrIntersectionTypeNode
{
    public IntersectionTypeNode() => Kind = TypeScriptSyntaxKind.IntersectionType;

    public NodeArray<ITypeNode> Types { get; set; }
}

public class ParenthesizedTypeNode : TypeNode
{
    public ParenthesizedTypeNode() => Kind = TypeScriptSyntaxKind.ParenthesizedType;

    public ITypeNode Type { get; set; }
}

public sealed class TypeOperatorNode : ParenthesizedTypeNode
{
    public TypeOperatorNode() => Kind = TypeScriptSyntaxKind.TypeOperator;

    public TypeScriptSyntaxKind Operator { get; set; } = TypeScriptSyntaxKind.KeyOfKeyword;
}

public sealed class IndexedAccessTypeNode : TypeNode
{
    public IndexedAccessTypeNode() => Kind = TypeScriptSyntaxKind.IndexedAccessType;

    public ITypeNode ObjectType { get; set; }
    public ITypeNode IndexType { get; set; }
}

public sealed class MappedTypeNode : Node, ITypeNode, IDeclaration
{
    public MappedTypeNode() => Kind = TypeScriptSyntaxKind.MappedType;

    public ReadonlyToken ReadonlyToken { get; set; }
    public TypeParameterDeclaration TypeParameter { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class LiteralTypeNode : TypeNode
{
    public LiteralTypeNode() => Kind = TypeScriptSyntaxKind.LiteralType;

    public IExpression Literal { get; set; }
}

public sealed class StringLiteral : LiteralExpression, IPropertyName
{
    public StringLiteral() => Kind = TypeScriptSyntaxKind.StringLiteral;

    public Node TextSourceNode { get; set; }
}

public interface IExpression : IBlockOrExpression, IVariableDeclarationListOrExpression
{
    object ExpressionBrand { get; set; }
}

public class Expression : Node, IExpression
{
    public object ExpressionBrand { get; set; }
}

public sealed class OmittedExpression : Expression, IArrayBindingElement
{
    public OmittedExpression() => Kind = TypeScriptSyntaxKind.OmittedExpression;
}

public sealed class PartiallyEmittedExpression : LeftHandSideExpression
{
    public PartiallyEmittedExpression() => Kind = TypeScriptSyntaxKind.PartiallyEmittedExpression;

    public IExpression Expression { get; set; }
}

public interface IUnaryExpression : IExpression
{
    object UnaryExpressionBrand { get; set; }
}

public class UnaryExpression : Expression, IUnaryExpression
{
    public object UnaryExpressionBrand { get; set; }
}

public interface IIncrementExpression : IUnaryExpression
{
    object IncrementExpressionBrand { get; set; }
}

public class IncrementExpression : UnaryExpression, IIncrementExpression
{
    public object IncrementExpressionBrand { get; set; }
}

public sealed class PrefixUnaryExpression : IncrementExpression
{
    public PrefixUnaryExpression() => Kind = TypeScriptSyntaxKind.PrefixUnaryExpression;

    public TypeScriptSyntaxKind Operator { get; set; }
    public IExpression Operand { get; set; }
}

public sealed class PostfixUnaryExpression : IncrementExpression
{
    public PostfixUnaryExpression() => Kind = TypeScriptSyntaxKind.PostfixUnaryExpression;

    public IExpression Operand { get; set; }
    public TypeScriptSyntaxKind Operator { get; set; }
}

public interface ILeftHandSideExpression : IIncrementExpression
{
    object LeftHandSideExpressionBrand { get; set; }
}

public class LeftHandSideExpression : IncrementExpression, ILeftHandSideExpression
{
    public object LeftHandSideExpressionBrand { get; set; }
}

public interface IMemberExpression : ILeftHandSideExpression
{
    object MemberExpressionBrand { get; set; }
}

public class MemberExpression : LeftHandSideExpression, IMemberExpression
{
    public object MemberExpressionBrand { get; set; }
}

public interface IPrimaryExpression : IMemberExpression
{
    object PrimaryExpressionBrand { get; set; }
}

public class PrimaryExpression : MemberExpression, IPrimaryExpression, IJsxTagNameExpression
{
    public object PrimaryExpressionBrand { get; set; }
}

public sealed class NullLiteral : Node, IPrimaryExpression, ITypeNode
{
    public NullLiteral() => Kind = TypeScriptSyntaxKind.NullKeyword;

    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class BooleanLiteral : Node, IPrimaryExpression, ITypeNode
{
    public BooleanLiteral() => Kind = TypeScriptSyntaxKind.BooleanKeyword;

    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
    public object TypeNodeBrand { get; set; }
}

public sealed class ThisExpression : Node, IPrimaryExpression, IKeywordTypeNode
{
    public ThisExpression() => Kind = TypeScriptSyntaxKind.ThisKeyword;

    public object TypeNodeBrand { get; set; }
    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
}

public sealed class SuperExpression : PrimaryExpression
{
    public SuperExpression() => Kind = TypeScriptSyntaxKind.SuperKeyword;
}

public sealed class DeleteExpression : UnaryExpression
{
    public DeleteExpression() => Kind = TypeScriptSyntaxKind.DeleteExpression;

    public IExpression Expression { get; set; }
}

public sealed class TypeOfExpression : UnaryExpression
{
    public TypeOfExpression() => Kind = TypeScriptSyntaxKind.TypeOfExpression;

    public IExpression Expression { get; set; }
}

public sealed class VoidExpression : UnaryExpression
{
    public VoidExpression() => Kind = TypeScriptSyntaxKind.VoidExpression;

    public IExpression Expression { get; set; }
}

public sealed class AwaitExpression : UnaryExpression
{
    public AwaitExpression() => Kind = TypeScriptSyntaxKind.AwaitExpression;

    public IExpression Expression { get; set; }
}

public sealed class YieldExpression : Expression
{
    public YieldExpression() => Kind = TypeScriptSyntaxKind.YieldExpression;

    public AsteriskToken AsteriskToken { get; set; }
    public IExpression Expression { get; set; }
}

public class BinaryExpression : Node, IExpression, IDeclaration
{
    public BinaryExpression() => Kind = TypeScriptSyntaxKind.BinaryExpression;

    public IExpression Left { get; set; }
    public Token OperatorToken { get; set; }
    public IExpression Right { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object ExpressionBrand { get; set; }
}

public class AssignmentExpression : BinaryExpression
{
}

public sealed class ObjectDestructuringAssignment : AssignmentExpression
{
}

public sealed class ArrayDestructuringAssignment : AssignmentExpression
{
}

public sealed class ConditionalExpression : Expression
{
    public ConditionalExpression() => Kind = TypeScriptSyntaxKind.ConditionalExpression;

    public IExpression Condition { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IExpression WhenTrue { get; set; }
    public ColonToken ColonToken { get; set; }
    public IExpression WhenFalse { get; set; }
}

public sealed class FunctionExpression : Node, IPrimaryExpression, IFunctionLikeDeclaration
{
    public FunctionExpression() => Kind = TypeScriptSyntaxKind.FunctionExpression;

    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
}

public interface IBlockOrExpression : INode
{
}

public sealed class ArrowFunction : Node, IExpression, IFunctionLikeDeclaration
{
    public ArrowFunction() => Kind = TypeScriptSyntaxKind.ArrowFunction;

    public EqualsGreaterThanToken EqualsGreaterThanToken { get; set; }
    public object ExpressionBrand { get; set; }
    public object FunctionLikeDeclarationBrand { get; set; }
    public AsteriskToken AsteriskToken { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public IBlockOrExpression Body { get; set; }
    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
}

public interface ILiteralLikeNode : INode
{
    string Text { get; set; }
    bool IsUnterminated { get; set; }
    bool HasExtendedUnicodeEscape { get; set; }
    bool IsOctalLiteral { get; set; }
}

public class LiteralLikeNode : Node, ILiteralLikeNode
{
    public string Text { get; set; }
    public bool IsUnterminated { get; set; }
    public bool HasExtendedUnicodeEscape { get; set; }
    public bool IsOctalLiteral { get; set; }
}

public interface ILiteralExpression : ILiteralLikeNode, IPrimaryExpression
{
    object LiteralExpressionBrand { get; set; }
}

public class LiteralExpression : Node, ILiteralExpression, IPrimaryExpression
{
    public object LiteralExpressionBrand { get; set; }
    public string Text { get; set; }
    public bool IsUnterminated { get; set; }
    public bool HasExtendedUnicodeEscape { get; set; }
    public bool IsOctalLiteral { get; set; }
    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }

    public object ExpressionBrand { get; set; }
}

public sealed class RegularExpressionLiteral : LiteralExpression
{
    public RegularExpressionLiteral() => Kind = TypeScriptSyntaxKind.RegularExpressionLiteral;
}

public sealed class NoSubstitutionTemplateLiteral : LiteralExpression
{
    public NoSubstitutionTemplateLiteral() => Kind = TypeScriptSyntaxKind.NoSubstitutionTemplateLiteral;
}

public sealed class NumericLiteral : LiteralExpression, IPropertyName
{
    public NumericLiteral() => Kind = TypeScriptSyntaxKind.NumericLiteral;
}

public sealed class TemplateHead : LiteralLikeNode
{
    public TemplateHead() => Kind = TypeScriptSyntaxKind.TemplateHead;
}

public sealed class TemplateMiddle : LiteralLikeNode
{
    public TemplateMiddle() => Kind = TypeScriptSyntaxKind.TemplateMiddle;
}

public sealed class TemplateTail : LiteralLikeNode
{
    public TemplateTail() => Kind = TypeScriptSyntaxKind.TemplateTail;
}

public sealed class TemplateExpression : PrimaryExpression
{
    public TemplateExpression() => Kind = TypeScriptSyntaxKind.TemplateExpression;

    public TemplateHead Head { get; set; }
    public NodeArray<TemplateSpan> TemplateSpans { get; set; }
}

public sealed class TemplateSpan : Node
{
    public TemplateSpan() => Kind = TypeScriptSyntaxKind.TemplateSpan;

    public IExpression Expression { get; set; }
    public ILiteralLikeNode Literal { get; set; }
}

public sealed class ParenthesizedExpression : PrimaryExpression
{
    public ParenthesizedExpression() => Kind = TypeScriptSyntaxKind.ParenthesizedExpression;

    public IExpression Expression { get; set; }
}

public sealed class ArrayLiteralExpression : PrimaryExpression
{
    public ArrayLiteralExpression() => Kind = TypeScriptSyntaxKind.ArrayLiteralExpression;

    public NodeArray<IExpression> Elements { get; set; }
    public bool MultiLine { get; set; }
}

public sealed class SpreadElement : Expression
{
    public SpreadElement() => Kind = TypeScriptSyntaxKind.SpreadElement;

    public IExpression Expression { get; set; }
}

public class ObjectLiteralExpressionBase<T> : Node, IPrimaryExpression, IDeclaration
{
    public NodeArray<T> Properties { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
}

public sealed class ObjectLiteralExpression : ObjectLiteralExpressionBase<IObjectLiteralElementLike>
{
    public ObjectLiteralExpression() => Kind = TypeScriptSyntaxKind.ObjectLiteralExpression;

    public bool MultiLine { get; set; }
}

public class PropertyAccessExpression : Node, IMemberExpression, IDeclaration, IJsxTagNameExpression
{
    public PropertyAccessExpression() => Kind = TypeScriptSyntaxKind.PropertyAccessExpression;

    public IExpression Expression { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
}

public sealed class SuperPropertyAccessExpression : PropertyAccessExpression
{
}

public sealed class PropertyAccessEntityNameExpression : PropertyAccessExpression
{
    public object PropertyAccessExpressionLikeQualifiedNameBrand { get; set; }
}

public class ElementAccessExpression : MemberExpression
{
    public ElementAccessExpression() => Kind = TypeScriptSyntaxKind.ElementAccessExpression;

    public IExpression Expression { get; set; }
    public IExpression ArgumentExpression { get; set; }
}

public sealed class SuperElementAccessExpression : ElementAccessExpression
{
}

public class CallExpression : Node, IMemberExpression, IDeclaration
{
    public CallExpression() => Kind = TypeScriptSyntaxKind.CallExpression;

    public IExpression Expression { get; set; }
    public NodeArray<ITypeNode> TypeArguments { get; set; }
    public NodeArray<IExpression> Arguments { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
}

public sealed class SuperCall : CallExpression
{
}

public sealed class ExpressionWithTypeArguments : TypeNode
{
    public ExpressionWithTypeArguments() => Kind = TypeScriptSyntaxKind.ExpressionWithTypeArguments;

    public IExpression Expression { get; set; }
    public NodeArray<ITypeNode> TypeArguments { get; set; }
}

public sealed class NewExpression : CallExpression, IPrimaryExpression, IDeclaration
{
    public NewExpression() => Kind = TypeScriptSyntaxKind.NewExpression;

    public object PrimaryExpressionBrand { get; set; }
}

public sealed class TaggedTemplateExpression : MemberExpression
{
    public TaggedTemplateExpression() => Kind = TypeScriptSyntaxKind.TaggedTemplateExpression;

    public IExpression Tag { get; set; }
    public Node Template { get; set; }
}

public sealed class AsExpression : Expression
{
    public AsExpression() => Kind = TypeScriptSyntaxKind.AsExpression;

    public IExpression Expression { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class TypeAssertion : UnaryExpression
{
    public TypeAssertion() => Kind = TypeScriptSyntaxKind.TypeAssertionExpression;

    public ITypeNode Type { get; set; }
    public IExpression Expression { get; set; }
}

public sealed class NonNullExpression : MemberExpression
{
    public NonNullExpression() => Kind = TypeScriptSyntaxKind.NonNullExpression;

    public IExpression Expression { get; set; }
}

public sealed class MetaProperty : PrimaryExpression
{
    public MetaProperty() => Kind = TypeScriptSyntaxKind.MetaProperty;

    public TypeScriptSyntaxKind KeywordToken { get; set; }
    public Identifier Name { get; set; }
}

public sealed class JsxElement : PrimaryExpression, IJsxChild
{
    public JsxElement() => Kind = TypeScriptSyntaxKind.JsxElement;

    public IExpression OpeningElement { get; set; }
    public NodeArray<IJsxChild> JsxChildren { get; set; }
    public JsxClosingElement ClosingElement { get; set; }
}

public sealed class JsxAttributes : ObjectLiteralExpressionBase<ObjectLiteralElement> // JsxAttributeLike>
{
    public JsxAttributes() => Kind = TypeScriptSyntaxKind.JsxAttributes;
}

public sealed class JsxOpeningElement : JsxSelfClosingElement
{
    public JsxOpeningElement() => Kind = TypeScriptSyntaxKind.JsxOpeningElement;
}

public class JsxSelfClosingElement : PrimaryExpression, IJsxChild
{
    public JsxSelfClosingElement() => Kind = TypeScriptSyntaxKind.JsxSelfClosingElement;

    public IJsxTagNameExpression TagName { get; set; }
    public JsxAttributes Attributes { get; set; }
}

public sealed class JsxAttribute : ObjectLiteralElement
{
    public JsxAttribute() => Kind = TypeScriptSyntaxKind.JsxAttribute;

    public Node Initializer { get; set; }
}

public sealed class JsxSpreadAttribute : ObjectLiteralElement
{
    public JsxSpreadAttribute() => Kind = TypeScriptSyntaxKind.JsxSpreadAttribute;

    public IExpression Expression { get; set; }
}

public sealed class JsxClosingElement : Node
{
    public JsxClosingElement() => Kind = TypeScriptSyntaxKind.JsxClosingElement;

    public IJsxTagNameExpression TagName { get; set; }
}

public sealed class JsxExpression : Expression, IJsxChild
{
    public JsxExpression() => Kind = TypeScriptSyntaxKind.JsxExpression;

    public Token DotDotDotToken { get; set; }
    public IExpression Expression { get; set; }
}

public sealed class JsxText : Node, IJsxChild
{
    public JsxText() => Kind = TypeScriptSyntaxKind.JsxText;
}

public interface IStatement : INode
{
    object StatementBrand { get; set; }
}

public class Statement : Node, IStatement
{
    public object StatementBrand { get; set; }
}

public sealed class NotEmittedStatement : Statement
{
    public NotEmittedStatement() => Kind = TypeScriptSyntaxKind.NotEmittedStatement;
}

public sealed class EndOfDeclarationMarker : Statement
{
    public EndOfDeclarationMarker() => Kind = TypeScriptSyntaxKind.EndOfDeclarationMarker;
}

public sealed class MergeDeclarationMarker : Statement
{
    public MergeDeclarationMarker() => Kind = TypeScriptSyntaxKind.MergeDeclarationMarker;
}

public sealed class EmptyStatement : Statement
{
    public EmptyStatement() => Kind = TypeScriptSyntaxKind.EmptyStatement;
}

public sealed class DebuggerStatement : Statement
{
    public DebuggerStatement() => Kind = TypeScriptSyntaxKind.DebuggerStatement;
}

public sealed class MissingDeclaration : Node, IDeclarationStatement, IClassElement, IObjectLiteralElement, ITypeElement
{
    public MissingDeclaration() => Kind = TypeScriptSyntaxKind.MissingDeclaration;

    public object ClassElementBrand { get; set; }
    public INode Name { get; set; }
    public object DeclarationBrand { get; set; }
    public object StatementBrand { get; set; }
    public object ObjectLiteralBrandBrand { get; set; }
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public class Block : Statement, IBlockOrExpression
{
    public Block() => Kind = TypeScriptSyntaxKind.Block;

    public NodeArray<IStatement> Statements { get; set; }
    public bool MultiLine { get; set; }
}

public sealed class VariableStatement : Statement
{
    public VariableStatement() => Kind = TypeScriptSyntaxKind.VariableStatement;

    public IVariableDeclarationList DeclarationList { get; set; }
}

public class ExpressionStatement : Statement
{
    public ExpressionStatement() => Kind = TypeScriptSyntaxKind.ExpressionStatement;

    public IExpression Expression { get; set; }
}

public sealed class PrologueDirective : ExpressionStatement
{
}

public sealed class IfStatement : Statement
{
    public IfStatement() => Kind = TypeScriptSyntaxKind.IfStatement;

    public IExpression Expression { get; set; }
    public IStatement ThenStatement { get; set; }
    public IStatement ElseStatement { get; set; }
}

public class IterationStatement : Statement
{
    public IStatement Statement { get; set; }
}

public sealed class DoStatement : IterationStatement
{
    public DoStatement() => Kind = TypeScriptSyntaxKind.DoStatement;

    public IExpression Expression { get; set; }
}

public sealed class WhileStatement : IterationStatement
{
    public WhileStatement() => Kind = TypeScriptSyntaxKind.WhileStatement;

    public IExpression Expression { get; set; }
}

public sealed class ForStatement : IterationStatement
{
    public ForStatement() => Kind = TypeScriptSyntaxKind.ForStatement;

    public IVariableDeclarationListOrExpression Initializer { get; set; }
    public IExpression Condition { get; set; }
    public IExpression Incrementor { get; set; }
}

public sealed class ForInStatement : IterationStatement
{
    public ForInStatement() => Kind = TypeScriptSyntaxKind.ForInStatement;

    public IVariableDeclarationListOrExpression Initializer { get; set; }
    public IExpression Expression { get; set; }
}

public sealed class ForOfStatement : IterationStatement
{
    public ForOfStatement() => Kind = TypeScriptSyntaxKind.ForOfStatement;

    public AwaitKeywordToken AwaitModifier { get; set; }
    public IVariableDeclarationListOrExpression Initializer { get; set; }
    public IExpression Expression { get; set; }
}

public sealed class BreakStatement : Statement, IBreakOrContinueStatement
{
    public BreakStatement() => Kind = TypeScriptSyntaxKind.BreakStatement;

    public Identifier Label { get; set; }
}

public sealed class ContinueStatement : Statement, IBreakOrContinueStatement
{
    public ContinueStatement() => Kind = TypeScriptSyntaxKind.ContinueStatement;

    public Identifier Label { get; set; }
}

public sealed class ReturnStatement : Statement
{
    public ReturnStatement() => Kind = TypeScriptSyntaxKind.ReturnStatement;

    public IExpression Expression { get; set; }
}

public sealed class WithStatement : Statement
{
    public WithStatement() => Kind = TypeScriptSyntaxKind.WithStatement;

    public IExpression Expression { get; set; }
    public IStatement Statement { get; set; }
}

public sealed class SwitchStatement : Statement
{
    public SwitchStatement() => Kind = TypeScriptSyntaxKind.SwitchStatement;

    public IExpression Expression { get; set; }
    public CaseBlock CaseBlock { get; set; }
    public bool PossiblyExhaustive { get; set; }
}

public sealed class CaseBlock : Node
{
    public CaseBlock() => Kind = TypeScriptSyntaxKind.CaseBlock;

    public NodeArray<ICaseOrDefaultClause> Clauses { get; set; }
}

public sealed class CaseClause : Node, ICaseOrDefaultClause
{
    public CaseClause() => Kind = TypeScriptSyntaxKind.CaseClause;

    public IExpression Expression { get; set; }
    public NodeArray<IStatement> Statements { get; set; }
}

public sealed class DefaultClause : Node, ICaseOrDefaultClause
{
    public DefaultClause() => Kind = TypeScriptSyntaxKind.DefaultClause;

    public NodeArray<IStatement> Statements { get; set; }
}

public sealed class LabeledStatement : Statement
{
    public LabeledStatement() => Kind = TypeScriptSyntaxKind.LabeledStatement;

    public Identifier Label { get; set; }
    public IStatement Statement { get; set; }
}

public sealed class ThrowStatement : Statement
{
    public ThrowStatement() => Kind = TypeScriptSyntaxKind.ThrowStatement;

    public IExpression Expression { get; set; }
}

public sealed class TryStatement : Statement
{
    public TryStatement() => Kind = TypeScriptSyntaxKind.TryStatement;

    public Block TryBlock { get; set; }
    public CatchClause CatchClause { get; set; }
    public Block FinallyBlock { get; set; }
}

public sealed class CatchClause : Node
{
    public CatchClause() => Kind = TypeScriptSyntaxKind.CatchClause;

    public VariableDeclaration VariableDeclaration { get; set; }
    public Block Block { get; set; }
}

public interface IClassLikeDeclaration : IDeclaration
{
    NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    NodeArray<HeritageClause> HeritageClauses { get; set; }
    NodeArray<IClassElement> Members { get; set; }
}

public sealed class ClassLikeDeclaration : Declaration, IClassLikeDeclaration
{
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<HeritageClause> HeritageClauses { get; set; }
    public NodeArray<IClassElement> Members { get; set; }
}

public sealed class ClassDeclaration : Node, IClassLikeDeclaration, IDeclarationStatement
{
    public ClassDeclaration() => Kind = TypeScriptSyntaxKind.ClassDeclaration;

    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<HeritageClause> HeritageClauses { get; set; }
    public NodeArray<IClassElement> Members { get; set; }
    public object DeclarationBrand { get; set; }
    public object StatementBrand { get; set; }
}

public sealed class ClassExpression : Node, IClassLikeDeclaration, IPrimaryExpression
{
    public ClassExpression() => Kind = TypeScriptSyntaxKind.ClassExpression;

    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<HeritageClause> HeritageClauses { get; set; }
    public NodeArray<IClassElement> Members { get; set; }
    public object DeclarationBrand { get; set; }
    public object PrimaryExpressionBrand { get; set; }
    public object MemberExpressionBrand { get; set; }
    public object LeftHandSideExpressionBrand { get; set; }
    public object IncrementExpressionBrand { get; set; }
    public object UnaryExpressionBrand { get; set; }
    public object ExpressionBrand { get; set; }
}

public interface IClassElement : IDeclaration
{
    object ClassElementBrand { get; set; }
}

public class ClassElement : Declaration, IClassElement
{
    public object ClassElementBrand { get; set; }
}

public interface ITypeElement : IDeclaration
{
    object TypeElementBrand { get; set; }
    QuestionToken QuestionToken { get; set; }
}

public class TypeElement : Declaration, ITypeElement
{
    public object TypeElementBrand { get; set; }
    public QuestionToken QuestionToken { get; set; }
}

public sealed class InterfaceDeclaration : DeclarationStatement
{
    public InterfaceDeclaration() => Kind = TypeScriptSyntaxKind.InterfaceDeclaration;

    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<HeritageClause> HeritageClauses { get; set; }
    public NodeArray<ITypeElement> Members { get; set; }
}

public sealed class HeritageClause : Node
{
    public HeritageClause() => Kind = TypeScriptSyntaxKind.HeritageClause;

    public TypeScriptSyntaxKind Token { get; set; }
    public NodeArray<ExpressionWithTypeArguments> Types { get; set; }
}

public sealed class TypeAliasDeclaration : DeclarationStatement
{
    public TypeAliasDeclaration() => Kind = TypeScriptSyntaxKind.TypeAliasDeclaration;

    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public ITypeNode Type { get; set; }
}

public sealed class EnumMember : Declaration
{
    public EnumMember() => Kind = TypeScriptSyntaxKind.EnumMember;

    public IExpression Initializer { get; set; }
}

public sealed class EnumDeclaration : DeclarationStatement
{
    public EnumDeclaration() => Kind = TypeScriptSyntaxKind.EnumDeclaration;

    public NodeArray<EnumMember> Members { get; set; }
}

public class ModuleDeclaration : DeclarationStatement
{
    public ModuleDeclaration() => Kind = TypeScriptSyntaxKind.ModuleDeclaration;

    public INode Body { get; set; }
}

public sealed class NamespaceDeclaration : ModuleDeclaration
{
}

public sealed class JsDocNamespaceDeclaration : ModuleDeclaration
{
}

public sealed class ModuleBlock : Block
{
    public ModuleBlock() => Kind = TypeScriptSyntaxKind.ModuleBlock;
}

public sealed class ImportEqualsDeclaration : DeclarationStatement
{
    public ImportEqualsDeclaration() => Kind = TypeScriptSyntaxKind.ImportEqualsDeclaration;

    public /*ModuleReference*/INode ModuleReference { get; set; }
}

public sealed class ExternalModuleReference : Node
{
    public ExternalModuleReference() => Kind = TypeScriptSyntaxKind.ExternalModuleReference;

    public IExpression Expression { get; set; }
}

public sealed class ImportDeclaration : Statement
{
    public ImportDeclaration() => Kind = TypeScriptSyntaxKind.ImportDeclaration;

    public ImportClause ImportClause { get; set; }
    public IExpression ModuleSpecifier { get; set; }
}

public sealed class ImportClause : Declaration
{
    public ImportClause() => Kind = TypeScriptSyntaxKind.ImportClause;

    public INamedImportBindings NamedBindings { get; set; }
}

public sealed class NamespaceImport : Declaration, INamedImportBindings
{
    public NamespaceImport() => Kind = TypeScriptSyntaxKind.NamespaceImport;
}

public sealed class NamespaceExportDeclaration : DeclarationStatement
{
    public NamespaceExportDeclaration() => Kind = TypeScriptSyntaxKind.NamespaceExportDeclaration;
}

public sealed class ExportDeclaration : DeclarationStatement
{
    public ExportDeclaration() => Kind = TypeScriptSyntaxKind.ExportDeclaration;

    public NamedExports ExportClause { get; set; }
    public IExpression ModuleSpecifier { get; set; }
}

public sealed class NamedImports : Node, INamedImportsOrExports, INamedImportBindings
{
    public NamedImports() => Kind = TypeScriptSyntaxKind.NamedImports;

    public NodeArray<ImportSpecifier> Elements { get; set; }
}

public sealed class NamedExports : Node, INamedImportsOrExports
{
    public NamedExports() => Kind = TypeScriptSyntaxKind.NamedExports;

    public NodeArray<ExportSpecifier> Elements { get; set; }
}

public sealed class ImportSpecifier : Declaration, IImportOrExportSpecifier
{
    public ImportSpecifier() => Kind = TypeScriptSyntaxKind.ImportSpecifier;

    public Identifier PropertyName { get; set; }
}

public sealed class ExportSpecifier : Declaration, IImportOrExportSpecifier
{
    public ExportSpecifier() => Kind = TypeScriptSyntaxKind.ExportSpecifier;

    public Identifier PropertyName { get; set; }
}

public sealed class ExportAssignment : DeclarationStatement
{
    public ExportAssignment() => Kind = TypeScriptSyntaxKind.ExportAssignment;

    public bool IsExportEquals { get; set; }
    public IExpression Expression { get; set; }
}

public sealed class FileReference : TextRange
{
    public string FileName { get; set; }
}

public sealed class CheckJsDirective : TextRange
{
    public bool Enabled { get; set; }
}

public class CommentRange : TextRange
{
    public bool HasTrailingNewLine { get; set; }
    public TypeScriptSyntaxKind Kind { get; set; }
}

public sealed class SynthesizedComment : CommentRange
{
    public string Text { get; set; }
}

public sealed class JsDocTypeExpression : Node
{
    public JsDocTypeExpression() => Kind = TypeScriptSyntaxKind.JsDocTypeExpression;

    public IJsDocType Type { get; set; }
}

public interface IJsDocType : ITypeNode
{
    object JsDocTypeBrand { get; set; }
}

public class JsDocType : TypeNode, IJsDocType
{
    public object JsDocTypeBrand { get; set; }
}

public sealed class JsDocAllType : JsDocType
{
    public JsDocAllType() => Kind = TypeScriptSyntaxKind.JsDocAllType;
}

public sealed class JsDocUnknownType : JsDocType
{
    public JsDocUnknownType() => Kind = TypeScriptSyntaxKind.JsDocUnknownType;
}

public sealed class JsDocArrayType : JsDocType
{
    public JsDocArrayType() => Kind = TypeScriptSyntaxKind.JsDocArrayType;

    public IJsDocType ElementType { get; set; }
}

public sealed class JsDocUnionType : JsDocType
{
    public JsDocUnionType() => Kind = TypeScriptSyntaxKind.JsDocUnionType;

    public NodeArray<IJsDocType> Types { get; set; }
}

public sealed class JsDocTupleType : JsDocType
{
    public JsDocTupleType() => Kind = TypeScriptSyntaxKind.JsDocTupleType;

    public NodeArray<IJsDocType> Types { get; set; }
}

public sealed class JsDocNonNullableType : JsDocType
{
    public JsDocNonNullableType() => Kind = TypeScriptSyntaxKind.JsDocNonNullableType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocNullableType : JsDocType
{
    public JsDocNullableType() => Kind = TypeScriptSyntaxKind.JsDocNullableType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocRecordType : JsDocType
{
    public JsDocRecordType() => Kind = TypeScriptSyntaxKind.JsDocRecordType;

    public TypeLiteralNode Literal { get; set; }
}

public sealed class JsDocTypeReference : JsDocType
{
    public JsDocTypeReference() => Kind = TypeScriptSyntaxKind.JsDocTypeReference;

    public IEntityName Name { get; set; }
    public NodeArray<IJsDocType> TypeArguments { get; set; }
}

public sealed class JsDocOptionalType : JsDocType
{
    public JsDocOptionalType() => Kind = TypeScriptSyntaxKind.JsDocOptionalType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocFunctionType : Node, IJsDocType, ISignatureDeclaration
{
    public JsDocFunctionType() => Kind = TypeScriptSyntaxKind.JsDocFunctionType;

    public object JsDocTypeBrand { get; set; }
    public object TypeNodeBrand { get; set; }
    public INode Name { get; set; }
    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
    public NodeArray<ParameterDeclaration> Parameters { get; set; }
    public ITypeNode Type { get; set; }
    public object DeclarationBrand { get; set; }
}

public sealed class JsDocVariadicType : JsDocType
{
    public JsDocVariadicType() => Kind = TypeScriptSyntaxKind.JsDocVariadicType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocConstructorType : JsDocType
{
    public JsDocConstructorType() => Kind = TypeScriptSyntaxKind.JsDocConstructorType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocThisType : JsDocType
{
    public JsDocThisType() => Kind = TypeScriptSyntaxKind.JsDocThisType;

    public IJsDocType Type { get; set; }
}

public sealed class JsDocLiteralType : JsDocType
{
    public JsDocLiteralType() => Kind = TypeScriptSyntaxKind.JsDocLiteralType;

    public LiteralTypeNode Literal { get; set; }
}

public sealed class JsDocRecordMember : PropertySignature
{
    public JsDocRecordMember() => Kind = TypeScriptSyntaxKind.JsDocRecordMember;
}

public sealed class JsDoc : Node
{
    public NodeArray<IJsDocTag> Tags { get; set; }
    public string Comment { get; set; }
}

public interface IJsDocTag : INode
{
    AtToken AtToken { get; set; }
    Identifier TagName { get; set; }
    string Comment { get; set; }
}

public class JsDocTag : Node, IJsDocTag
{
    public JsDocTag() => Kind = TypeScriptSyntaxKind.JsDocTag;

    public AtToken AtToken { get; set; }
    public Identifier TagName { get; set; }
    public string Comment { get; set; }
}

public sealed class JsDocUnknownTag : JsDocTag
{
}

public sealed class JsDocAugmentsTag : JsDocTag
{
    public JsDocAugmentsTag() => Kind = TypeScriptSyntaxKind.JsDocAugmentsTag;

    public JsDocTypeExpression TypeExpression { get; set; }
}

public sealed class JsDocTemplateTag : JsDocTag
{
    public JsDocTemplateTag() => Kind = TypeScriptSyntaxKind.JsDocTemplateTag;

    public NodeArray<TypeParameterDeclaration> TypeParameters { get; set; }
}

public sealed class JsDocReturnTag : JsDocTag
{
    public JsDocReturnTag() => Kind = TypeScriptSyntaxKind.JsDocReturnTag;

    public JsDocTypeExpression TypeExpression { get; set; }
}

public sealed class JsDocTypeTag : JsDocTag
{
    public JsDocTypeTag() => Kind = TypeScriptSyntaxKind.JsDocTypeTag;

    public JsDocTypeExpression TypeExpression { get; set; }
}

public sealed class JsDocTypedefTag : Node, IJsDocTag, IDeclaration
{
    public JsDocTypedefTag() => Kind = TypeScriptSyntaxKind.JsDocTypedefTag;

    public INode FullName { get; set; }
    public JsDocTypeExpression TypeExpression { get; set; }
    public JsDocTypeLiteral JsDocTypeLiteral { get; set; }
    public object DeclarationBrand { get; set; }
    public INode Name { get; set; }
    public AtToken AtToken { get; set; }
    public Identifier TagName { get; set; }
    public string Comment { get; set; }
}

public sealed class JsDocPropertyTag : Node, IJsDocTag, ITypeElement
{
    public JsDocPropertyTag() => Kind = TypeScriptSyntaxKind.JsDocPropertyTag;

    public JsDocTypeExpression TypeExpression { get; set; }
    public AtToken AtToken { get; set; }
    public Identifier TagName { get; set; }
    public string Comment { get; set; }
    public object TypeElementBrand { get; set; }
    public INode Name { get; set; }
    public QuestionToken QuestionToken { get; set; }
    public object DeclarationBrand { get; set; }
}

public sealed class JsDocTypeLiteral : JsDocType
{
    public JsDocTypeLiteral() => Kind = TypeScriptSyntaxKind.JsDocTypeLiteral;

    public NodeArray<JsDocPropertyTag> JsDocPropertyTags { get; set; }
    public JsDocTypeTag JsDocTypeTag { get; set; }
}

public sealed class JsDocParameterTag : JsDocTag
{
    public JsDocParameterTag() => Kind = TypeScriptSyntaxKind.JsDocParameterTag;

    public Identifier PreParameterName { get; set; }
    public JsDocTypeExpression TypeExpression { get; set; }
    public Identifier PostParameterName { get; set; }
    public Identifier ParameterName { get; set; }
    public bool IsBracketed { get; set; }
}

public interface IFlowLock
{
    bool Locked { get; set; }
}

public sealed class FlowLock : IFlowLock
{
    public bool Locked { get; set; }
}

public sealed class AfterFinallyFlow : IFlowNode, IFlowLock
{
    public FlowNode Antecedent { get; set; }
    public bool Locked { get; set; }
    public FlowFlags Flags { get; set; }
    public int Id { get; set; }
}

public sealed class PreFinallyFlow : FlowNode
{
    public FlowNode Antecedent { get; set; }
    public FlowLock Lock { get; set; }
}

public interface IFlowNode
{
    FlowFlags Flags { get; set; }
    int Id { get; set; }
}

public class FlowNode : IFlowNode
{
    public FlowFlags Flags { get; set; }
    public int Id { get; set; }
}

public sealed class FlowStart : FlowNode
{
    public Node Container { get; set; }
}

public sealed class FlowLabel : FlowNode
{
    public FlowNode[] Antecedents { get; set; }
}

public sealed class FlowAssignment : FlowNode
{
    public Node Node { get; set; }
    public FlowNode Antecedent { get; set; }
}

public sealed class FlowCondition : FlowNode
{
    public IExpression Expression { get; set; }
    public FlowNode Antecedent { get; set; }
}

public sealed class FlowSwitchClause : FlowNode
{
    public SwitchStatement SwitchStatement { get; set; }
    public int ClauseStart { get; set; }
    public int ClauseEnd { get; set; }
    public FlowNode Antecedent { get; set; }
}

public sealed class FlowArrayMutation : FlowNode
{
    public Node Node { get; set; }
    public FlowNode Antecedent { get; set; }
}

public sealed class IncompleteType
{
    public TypeFlags Flags { get; set; }
    public TypeScriptType Type { get; set; }
}

public sealed class AmdDependency
{
    public string Path { get; set; }
    public string Name { get; set; }
}

public interface ISourceFileLike
{
    string Text { get; set; }
    int[] LineMap { get; set; }
}

public class SourceFile : Declaration, ISourceFileLike
{
    public SourceFile() => Kind = TypeScriptSyntaxKind.SourceFile;

    public NodeArray<IStatement> Statements { get; set; }
    public Token EndOfFileToken { get; set; }
    public string FileName { get; set; }
    public AmdDependency[] AmdDependencies { get; set; }
    public string ModuleName { get; set; }
    public FileReference[] ReferencedFiles { get; set; }
    public FileReference[] TypeReferenceDirectives { get; set; }
    public LanguageVariant LanguageVariant { get; set; }
    public bool IsDeclarationFile { get; set; }
    public Map<string> RenamedDependencies { get; set; }
    public bool HasNoDefaultLib { get; set; }
    public ScriptTarget LanguageVersion { get; set; }
    public ScriptKind ScriptKind { get; set; }
    public INode ExternalModuleIndicator { get; set; }
    public Node CommonJsModuleIndicator { get; set; }
    public List<string> Identifiers { get; set; }
    public int NodeCount { get; set; }
    public int IdentifierCount { get; set; }
    public int SymbolCount { get; set; }
    public List<TypeScriptDiagnostic> ParseDiagnostics { get; set; }
    public List<TypeScriptDiagnostic> AdditionalSyntacticDiagnostics { get; set; }
    public List<TypeScriptDiagnostic> BindDiagnostics { get; set; }
    public Map<string> ClassifiableNames { get; set; }
    public Map<ResolvedModuleFull> ResolvedModules { get; set; }
    public Map<ResolvedTypeReferenceDirective> ResolvedTypeReferenceDirectiveNames { get; set; }
    public LiteralExpression[] Imports { get; set; }
    public LiteralExpression[] ModuleAugmentations { get; set; }
    public PatternAmbientModule[] PatternAmbientModules { get; set; }
    public string[] AmbientModuleNames { get; set; }
    public TextRange CheckJsDirective { get; set; }
    public string Text { get; set; }
    public int[] LineMap { get; set; }
}

public sealed class Bundle : Node
{
    public Bundle() => Kind = TypeScriptSyntaxKind.Bundle;

    public SourceFile[] SourceFiles { get; set; }
}

public class ScriptReferenceHost
{
}

public sealed class ParseConfigHost
{
    public bool UseCaseSensitiveFileNames { get; set; }
}

public sealed class WriteFileCallback
{
}

public sealed class CancellationToken
{
}

public sealed class Program : ScriptReferenceHost
{
    public bool StructureIsReused { get; set; }
}

public sealed class CustomTransformers
{
    public TransformerFactory<SourceFile>[] Before { get; set; }
    public TransformerFactory<SourceFile>[] After { get; set; }
}

public sealed class SourceMapSpan
{
    public int EmittedLine { get; set; }
    public int EmittedColumn { get; set; }
    public int SourceLine { get; set; }
    public int SourceColumn { get; set; }
    public int NameIndex { get; set; }
    public int SourceIndex { get; set; }
}

public sealed class SourceMapData
{
    public string SourceMapFilePath { get; set; }
    public string JsSourceMappingUrl { get; set; }
    public string SourceMapFile { get; set; }
    public string SourceMapSourceRoot { get; set; }
    public string[] SourceMapSources { get; set; }
    public string[] SourceMapSourcesContent { get; set; }
    public string[] InputSourceFileNames { get; set; }
    public string[] SourceMapNames { get; set; }
    public string SourceMapMappings { get; set; }
    public SourceMapSpan[] SourceMapDecodedMappings { get; set; }
}

public sealed class EmitResult
{
    public bool EmitSkipped { get; set; }
    public TypeScriptDiagnostic[] Diagnostics { get; set; }
    public string[] EmittedFiles { get; set; }
    public SourceMapData[] SourceMaps { get; set; }
}

public sealed class TypeCheckerHost
{
}

public sealed class TypeChecker
{
}

public sealed class SymbolDisplayBuilder
{
}

public sealed class SymbolWriter
{
}

public class TypePredicateBase
{
    public TypePredicateKind Kind { get; set; }
    public TypeScriptType Type { get; set; }
}

public sealed class ThisTypePredicate : TypePredicateBase
{
    public ThisTypePredicate() => Kind = TypePredicateKind.This;
}

public sealed class IdentifierTypePredicate : TypePredicateBase
{
    public IdentifierTypePredicate() => Kind = TypePredicateKind.Identifier;

    public string ParameterName { get; set; }
    public int ParameterIndex { get; set; }
}

public class SymbolVisibilityResult
{
    public SymbolAccessibility Accessibility { get; set; }
    public IAnyImportSyntax[] AliasesToMakeVisible { get; set; }
    public string ErrorSymbolName { get; set; }
    public Node ErrorNode { get; set; }
}

public sealed class SymbolAccessibilityResult : SymbolVisibilityResult
{
    public string ErrorModuleName { get; set; }
}

public sealed class EmitResolver
{
}

public interface ISymbol
{
    SymbolFlags Flags { get; set; }
    string Name { get; set; }
    Declaration[] Declarations { get; set; }
    Declaration ValueDeclaration { get; set; }
    SymbolTable Members { get; set; }
    SymbolTable Exports { get; set; }
    SymbolTable GlobalExports { get; set; }
    int Id { get; set; }
    int MergeId { get; set; }
    Symbol Parent { get; set; }
    Symbol ExportSymbol { get; set; }
    bool ConstEnumOnlyModule { get; set; }
    bool IsReferenced { get; set; }
    bool IsReplaceableByMethod { get; set; }
    bool IsAssigned { get; set; }
}

public sealed class Symbol : ISymbol
{
    public SymbolFlags Flags { get; set; }
    public string Name { get; set; }
    public Declaration[] Declarations { get; set; }
    public Declaration ValueDeclaration { get; set; }
    public SymbolTable Members { get; set; }
    public SymbolTable Exports { get; set; }
    public SymbolTable GlobalExports { get; set; }
    public int Id { get; set; }
    public int MergeId { get; set; }
    public Symbol Parent { get; set; }
    public Symbol ExportSymbol { get; set; }
    public bool ConstEnumOnlyModule { get; set; }
    public bool IsReferenced { get; set; }
    public bool IsReplaceableByMethod { get; set; }
    public bool IsAssigned { get; set; }
}

public interface ISymbolLinks
{
    Symbol Target { get; set; }
    TypeScriptType Type { get; set; }
    TypeScriptType DeclaredType { get; set; }
    TypeParameter[] TypeParameters { get; set; }
    TypeScriptType InferredClassType { get; set; }
    Map<TypeScriptType> Instantiations { get; set; }
    TypeMapper Mapper { get; set; }
    bool Referenced { get; set; }
    UnionOrIntersectionType ContainingType { get; set; }
    Symbol LeftSpread { get; set; }
    Symbol RightSpread { get; set; }
    Symbol MappedTypeOrigin { get; set; }
    bool IsDiscriminantProperty { get; set; }
    SymbolTable ResolvedExports { get; set; }
    bool ExportsChecked { get; set; }
    bool TypeParametersChecked { get; set; }
    bool IsDeclarationWithCollidingName { get; set; }
    BindingElement BindingElement { get; set; }
    bool ExportsSomeValue { get; set; }
}

public sealed class SymbolLinks : ISymbolLinks
{
    public Symbol Target { get; set; }
    public TypeScriptType Type { get; set; }
    public TypeScriptType DeclaredType { get; set; }
    public TypeParameter[] TypeParameters { get; set; }
    public TypeScriptType InferredClassType { get; set; }
    public Map<TypeScriptType> Instantiations { get; set; }
    public TypeMapper Mapper { get; set; }
    public bool Referenced { get; set; }
    public UnionOrIntersectionType ContainingType { get; set; }
    public Symbol LeftSpread { get; set; }
    public Symbol RightSpread { get; set; }
    public Symbol MappedTypeOrigin { get; set; }
    public bool IsDiscriminantProperty { get; set; }
    public SymbolTable ResolvedExports { get; set; }
    public bool ExportsChecked { get; set; }
    public bool TypeParametersChecked { get; set; }
    public bool IsDeclarationWithCollidingName { get; set; }
    public BindingElement BindingElement { get; set; }
    public bool ExportsSomeValue { get; set; }
}

public sealed class TransientSymbol : ISymbol, ISymbolLinks
{
    public CheckFlags CheckFlags { get; set; }
    public SymbolFlags Flags { get; set; }
    public string Name { get; set; }
    public Declaration[] Declarations { get; set; }
    public Declaration ValueDeclaration { get; set; }
    public SymbolTable Members { get; set; }
    public SymbolTable Exports { get; set; }
    public SymbolTable GlobalExports { get; set; }
    public int Id { get; set; }
    public int MergeId { get; set; }
    public Symbol Parent { get; set; }
    public Symbol ExportSymbol { get; set; }
    public bool ConstEnumOnlyModule { get; set; }
    public bool IsReferenced { get; set; }
    public bool IsReplaceableByMethod { get; set; }
    public bool IsAssigned { get; set; }
    public Symbol Target { get; set; }
    public TypeScriptType Type { get; set; }
    public TypeScriptType DeclaredType { get; set; }
    public TypeParameter[] TypeParameters { get; set; }
    public TypeScriptType InferredClassType { get; set; }
    public Map<TypeScriptType> Instantiations { get; set; }
    public TypeMapper Mapper { get; set; }
    public bool Referenced { get; set; }
    public UnionOrIntersectionType ContainingType { get; set; }
    public Symbol LeftSpread { get; set; }
    public Symbol RightSpread { get; set; }
    public Symbol MappedTypeOrigin { get; set; }
    public bool IsDiscriminantProperty { get; set; }
    public SymbolTable ResolvedExports { get; set; }
    public bool ExportsChecked { get; set; }
    public bool TypeParametersChecked { get; set; }
    public bool IsDeclarationWithCollidingName { get; set; }
    public BindingElement BindingElement { get; set; }
    public bool ExportsSomeValue { get; set; }
}

public sealed class Pattern
{
    public string Prefix { get; set; }
    public string Suffix { get; set; }
}

public sealed class PatternAmbientModule
{
    public Pattern Pattern { get; set; }
    public Symbol Symbol { get; set; }
}

public sealed class NodeLinks
{
    public NodeCheckFlags Flags { get; set; }
    public TypeScriptType ResolvedType { get; set; }
    public Signature ResolvedSignature { get; set; }
    public Symbol ResolvedSymbol { get; set; }
    public IndexInfo ResolvedIndexInfo { get; set; }
    public bool MaybeTypePredicate { get; set; }
    public int EnumMemberValue { get; set; }
    public bool IsVisible { get; set; }
    public bool HasReportedStatementInAmbientContext { get; set; }
    public JsxFlags JsxFlags { get; set; }
    public TypeScriptType ResolvedJsxElementAttributesType { get; set; }
    public bool HasSuperCall { get; set; }
    public ExpressionStatement SuperCall { get; set; }
    public TypeScriptType[] SwitchTypes { get; set; }
}

public interface IType
{
    TypeFlags Flags { get; set; }
    int Id { get; set; }
    Symbol Symbol { get; set; }
    IDestructuringPattern Pattern { get; set; }
    Symbol AliasSymbol { get; set; }
    TypeScriptType[] AliasTypeArguments { get; set; }
}

public class TypeScriptType : IType
{
    public TypeFlags Flags { get; set; }
    public int Id { get; set; }
    public Symbol Symbol { get; set; }
    public IDestructuringPattern Pattern { get; set; }
    public Symbol AliasSymbol { get; set; }
    public TypeScriptType[] AliasTypeArguments { get; set; }
}

public sealed class IntrinsicType : TypeScriptType
{
    public string IntrinsicName { get; set; }
}

public class LiteralType : TypeScriptType
{
    public string Text { get; set; }
    public LiteralType FreshType { get; set; }
    public LiteralType RegularType { get; set; }
}

public sealed class EnumType : TypeScriptType
{
    public EnumLiteralType[] MemberTypes { get; set; }
}

public sealed class EnumLiteralType : LiteralType
{
}

public interface IObjectType : IType
{
    ObjectFlags ObjectFlags { get; set; }
}

public class ObjectType : TypeScriptType, IObjectType
{
    public ObjectFlags ObjectFlags { get; set; }
}

public interface IInterfaceType : IObjectType
{
    TypeParameter[] TypeParameters { get; set; }
    TypeParameter[] OuterTypeParameters { get; set; }
    TypeParameter[] LocalTypeParameters { get; set; }
    TypeParameter ThisType { get; set; }
    TypeScriptType ResolvedBaseConstructorType { get; set; }
    IBaseType[] ResolvedBaseTypes { get; set; }
}

public class InterfaceType : ObjectType, IInterfaceType
{
    public TypeParameter[] TypeParameters { get; set; }
    public TypeParameter[] OuterTypeParameters { get; set; }
    public TypeParameter[] LocalTypeParameters { get; set; }
    public TypeParameter ThisType { get; set; }
    public TypeScriptType ResolvedBaseConstructorType { get; set; }
    public IBaseType[] ResolvedBaseTypes { get; set; }
}

public sealed class InterfaceTypeWithDeclaredMembers : InterfaceType
{
    public Symbol[] DeclaredProperties { get; set; }
    public Signature[] DeclaredCallSignatures { get; set; }
    public Signature[] DeclaredConstructSignatures { get; set; }
    public IndexInfo DeclaredStringIndexInfo { get; set; }
    public IndexInfo DeclaredNumberIndexInfo { get; set; }
}

public interface ITypeReference : IObjectType
{
    GenericType Target { get; set; }
    TypeScriptType[] TypeArguments { get; set; }
}

public sealed class TypeReference : ObjectType, ITypeReference
{
    public GenericType Target { get; set; }
    public TypeScriptType[] TypeArguments { get; set; }
}

public sealed class GenericType : ObjectType, IInterfaceType, ITypeReference
{
    public Map<TypeReference> Instantiations { get; set; }
    public TypeParameter[] TypeParameters { get; set; }
    public TypeParameter[] OuterTypeParameters { get; set; }
    public TypeParameter[] LocalTypeParameters { get; set; }
    public TypeParameter ThisType { get; set; }
    public TypeScriptType ResolvedBaseConstructorType { get; set; }
    public IBaseType[] ResolvedBaseTypes { get; set; }
    public GenericType Target { get; set; }
    public TypeScriptType[] TypeArguments { get; set; }
}

public interface IUnionOrIntersectionType : IType
{
    TypeScriptType[] Types { get; set; }
    SymbolTable PropertyCache { get; set; }
    Symbol[] ResolvedProperties { get; set; }
    IndexType ResolvedIndexType { get; set; }
    TypeScriptType ResolvedBaseConstraint { get; set; }
    bool CouldContainTypeVariables { get; set; }
}

public class UnionOrIntersectionType : TypeScriptType, IUnionOrIntersectionType
{
    public TypeScriptType[] Types { get; set; }
    public SymbolTable PropertyCache { get; set; }
    public Symbol[] ResolvedProperties { get; set; }
    public IndexType ResolvedIndexType { get; set; }
    public TypeScriptType ResolvedBaseConstraint { get; set; }
    public bool CouldContainTypeVariables { get; set; }
}

public interface IUnionType : IUnionOrIntersectionType
{
}

public class UnionType : UnionOrIntersectionType, IUnionType
{
}

public sealed class IntersectionType : UnionOrIntersectionType
{
    public TypeScriptType ResolvedApparentType { get; set; }
}

public sealed class AnonymousType : ObjectType
{
    public AnonymousType Target { get; set; }
    public TypeMapper Mapper { get; set; }
}

public sealed class MappedType : ObjectType
{
    public MappedTypeNode Declaration { get; set; }
    public TypeParameter TypeParameter { get; set; }
    public TypeScriptType ConstraintType { get; set; }
    public TypeScriptType TemplateType { get; set; }
    public TypeScriptType ModifiersType { get; set; }
    public TypeMapper Mapper { get; set; }
}

public sealed class EvolvingArrayType : ObjectType
{
    public TypeScriptType ElementType { get; set; }
    public TypeScriptType FinalArrayType { get; set; }
}

public class ResolvedType : TypeScriptType, IObjectType, IUnionOrIntersectionType
{
    public SymbolTable Members { get; set; }
    public Symbol[] Properties { get; set; }
    public Signature[] CallSignatures { get; set; }
    public Signature[] ConstructSignatures { get; set; }
    public IndexInfo StringIndexInfo { get; set; }
    public IndexInfo NumberIndexInfo { get; set; }
    public ObjectFlags ObjectFlags { get; set; }
    public TypeScriptType[] Types { get; set; }
    public SymbolTable PropertyCache { get; set; }
    public Symbol[] ResolvedProperties { get; set; }
    public IndexType ResolvedIndexType { get; set; }
    public TypeScriptType ResolvedBaseConstraint { get; set; }
    public bool CouldContainTypeVariables { get; set; }
}

public sealed class FreshObjectLiteralType : ResolvedType
{
    public ResolvedType RegularType { get; set; }
}

public sealed class IterableOrIteratorType : TypeScriptType, IObjectType, IUnionType
{
    public TypeScriptType IteratedTypeOfIterable { get; set; }
    public TypeScriptType IteratedTypeOfIterator { get; set; }
    public TypeScriptType IteratedTypeOfAsyncIterable { get; set; }
    public TypeScriptType IteratedTypeOfAsyncIterator { get; set; }
    public ObjectFlags ObjectFlags { get; set; }
    public TypeScriptType[] Types { get; set; }
    public SymbolTable PropertyCache { get; set; }
    public Symbol[] ResolvedProperties { get; set; }
    public IndexType ResolvedIndexType { get; set; }
    public TypeScriptType ResolvedBaseConstraint { get; set; }
    public bool CouldContainTypeVariables { get; set; }
}

public sealed class PromiseOrAwaitableType : TypeScriptType, IObjectType, IUnionType
{
    public TypeScriptType PromiseTypeOfPromiseConstructor { get; set; }
    public TypeScriptType PromisedTypeOfPromise { get; set; }
    public TypeScriptType AwaitedTypeOfType { get; set; }
    public ObjectFlags ObjectFlags { get; set; }
    public TypeScriptType[] Types { get; set; }
    public SymbolTable PropertyCache { get; set; }
    public Symbol[] ResolvedProperties { get; set; }
    public IndexType ResolvedIndexType { get; set; }
    public TypeScriptType ResolvedBaseConstraint { get; set; }
    public bool CouldContainTypeVariables { get; set; }
}

public class TypeVariable : TypeScriptType
{
    public TypeScriptType ResolvedBaseConstraint { get; set; }
    public IndexType ResolvedIndexType { get; set; }
}

public sealed class TypeParameter : TypeVariable
{
    public TypeScriptType Constraint { get; set; }
    public TypeScriptType Default { get; set; }
    public TypeParameter Target { get; set; }
    public TypeMapper Mapper { get; set; }
    public bool IsThisType { get; set; }
    public TypeScriptType ResolvedDefaultType { get; set; }
}

public sealed class IndexedAccessType : TypeVariable
{
    public TypeScriptType ObjectType { get; set; }
    public TypeScriptType IndexType { get; set; }
    public TypeScriptType Constraint { get; set; }
}

public sealed class IndexType : TypeScriptType
{
    public TypeScriptType Type { get; set; } // TypeVariable | UnionOrIntersectionType
}

public sealed class Signature
{
    public SignatureDeclaration Declaration { get; set; }
    public TypeParameter[] TypeParameters { get; set; }
    public Symbol[] Parameters { get; set; }
    public Symbol ThisParameter { get; set; }
    public TypeScriptType ResolvedReturnType { get; set; }
    public int MinArgumentCount { get; set; }
    public bool HasRestParameter { get; set; }
    public bool HasLiteralTypes { get; set; }
    public Signature Target { get; set; }
    public TypeMapper Mapper { get; set; }
    public Signature[] UnionSignatures { get; set; }
    public Signature ErasedSignatureCache { get; set; }
    public ObjectType IsolatedSignatureType { get; set; }
    public ITypePredicate TypePredicate { get; set; }
    public Map<Signature> Instantiations { get; set; }
}

public sealed class IndexInfo
{
    public TypeScriptType Type { get; set; }
    public bool IsReadonly { get; set; }
    public SignatureDeclaration Declaration { get; set; }
}

public sealed class TypeMapper
{
    public TypeScriptType[] MappedTypes { get; set; }
    public TypeScriptType[] Instantiations { get; set; }
    public InferenceContext Context { get; set; }
}

public sealed class TypeInferences
{
    public TypeScriptType[] Primary { get; set; }
    public TypeScriptType[] Secondary { get; set; }
    public bool TopLevel { get; set; }
    public bool IsFixed { get; set; }
}

public sealed class InferenceContext
{
    public Signature Signature { get; set; }
    public bool InferUnionTypes { get; set; }
    public TypeInferences[] Inferences { get; set; }
    public TypeScriptType[] InferredTypes { get; set; }
    public TypeMapper Mapper { get; set; }
    public int FailedTypeParameterIndex { get; set; }
    public bool UseAnyForNoInferences { get; set; }
}

public sealed class JsFileExtensionInfo
{
    public string Extension { get; set; }
    public bool IsMixedContent { get; set; }
}

public readonly record struct DiagnosticMessage
{
    public string Key { get; init; }
    public DiagnosticCategory Category { get; init; }
    public int Code { get; init; }
    public string Message { get; init; }

    public static DiagnosticMessage Warning(string key, int code, string message = default) =>
        new()
        {
            Key = key,
            Category = DiagnosticCategory.Warning,
            Code = code,
            Message = key ?? message
        };

    public static DiagnosticMessage Error(string key, int code, string message = default) =>
        new()
        {
            Key = key,
            Category = DiagnosticCategory.Error,
            Code = code,
            Message = key ?? message
        };

    public static DiagnosticMessage Info(string key, int code, string message = default) =>
        new()
        {
            Key = key,
            Category = DiagnosticCategory.Message,
            Code = code,
            Message = key ?? message
        };
}

public sealed class DiagnosticMessageChain
{
    public string MessageText { get; set; }
    public DiagnosticCategory Category { get; set; }
    public int Code { get; set; }
    public DiagnosticMessageChain Next { get; set; }
}

public sealed class PluginImport
{
    public string Name { get; set; }
}

public sealed class CompilerOptions
{
    public bool All { get; set; }
    public bool AllowJs { get; set; }
    public bool AllowNonTsExtensions { get; set; }
    public bool AllowSyntheticDefaultImports { get; set; }
    public bool AllowUnreachableCode { get; set; }
    public bool AllowUnusedLabels { get; set; }
    public bool AlwaysStrict { get; set; }
    public string BaseUrl { get; set; }
    public string Charset { get; set; }
    public bool CheckJs { get; set; }
    public string ConfigFilePath { get; set; }
    public bool Declaration { get; set; }
    public string DeclarationDir { get; set; }
    public bool Diagnostics { get; set; }
    public bool ExtendedDiagnostics { get; set; }
    public bool DisableSizeLimit { get; set; }
    public bool DownlevelIteration { get; set; }
    public bool EmitBom { get; set; }
    public bool EmitDecoratorMetadata { get; set; }
    public bool ExperimentalDecorators { get; set; }
    public bool ForceConsistentCasingInFileNames { get; set; }
    public bool Help { get; set; }
    public bool ImportHelpers { get; set; }
    public bool Init { get; set; }
    public bool InlineSourceMap { get; set; }
    public bool InlineSources { get; set; }
    public bool IsolatedModules { get; set; }
    public JsxEmit Jsx { get; set; }
    public string[] Lib { get; set; }
    public bool ListEmittedFiles { get; set; }
    public bool ListFiles { get; set; }
    public string Locale { get; set; }
    public string MapRoot { get; set; }
    public int MaxNodeModuleJsDepth { get; set; }
    public ModuleKind Module { get; set; }
    public ModuleResolutionKind ModuleResolution { get; set; }
    public NewLineKind NewLine { get; set; }
    public bool NoEmit { get; set; }
    public bool NoEmitForJsFiles { get; set; }
    public bool NoEmitHelpers { get; set; }
    public bool NoEmitOnError { get; set; }
    public bool NoErrorTruncation { get; set; }
    public bool NoFallthroughCasesInSwitch { get; set; }
    public bool NoImplicitAny { get; set; }
    public bool NoImplicitReturns { get; set; }
    public bool NoImplicitThis { get; set; }
    public bool NoUnusedLocals { get; set; }
    public bool NoUnusedParameters { get; set; }
    public bool NoImplicitUseStrict { get; set; }
    public bool NoLib { get; set; }
    public bool NoResolve { get; set; }
    public string Out { get; set; }
    public string OutDir { get; set; }
    public string OutFile { get; set; }
    public Map<string[]> Paths { get; set; }
    public PluginImport[] Plugins { get; set; }
    public bool PreserveConstEnums { get; set; }
    public string Project { get; set; }
    public DiagnosticStyle Pretty { get; set; }
    public string ReactNamespace { get; set; }
    public string JsxFactory { get; set; }
    public bool RemoveComments { get; set; }
    public string RootDir { get; set; }
    public string[] RootDirs { get; set; }
    public bool SkipLibCheck { get; set; }
    public bool SkipDefaultLibCheck { get; set; }
    public bool SourceMap { get; set; }
    public string SourceRoot { get; set; }
    public bool Strict { get; set; }
    public bool StrictNullChecks { get; set; }
    public bool StripInternal { get; set; }
    public bool SuppressExcessPropertyErrors { get; set; }
    public bool SuppressImplicitAnyIndexErrors { get; set; }
    public bool SuppressOutputPathCheck { get; set; }
    public ScriptTarget Target { get; set; }
    public bool TraceResolution { get; set; }
    public string[] Types { get; set; }
    public string[] TypeRoots { get; set; }
    public bool Version { get; set; }
    public bool Watch { get; set; }
}

public sealed class TypeAcquisition
{
    public bool EnableAutoDiscovery { get; set; }
    public bool Enable { get; set; }
    public string[] Include { get; set; }
    public string[] Exclude { get; set; }
}

public sealed class DiscoverTypingsInfo
{
    public string[] FileNames { get; set; }
    public string ProjectRootPath { get; set; }
    public string SafeListPath { get; set; }
    public Map<string> PackageNameToTypingLocation { get; set; }
    public TypeAcquisition TypeAcquisition { get; set; }
    public CompilerOptions CompilerOptions { get; set; }
    public IReadOnlyList<string> UnresolvedImports { get; set; }
}

public sealed class LineAndCharacter
{
    public int Line { get; set; }
    public int Character { get; set; }
}

public sealed class ParsedCommandLine
{
    public CompilerOptions Options { get; set; }
    public TypeAcquisition TypeAcquisition { get; set; }
    public string[] FileNames { get; set; }
    public object Raw { get; set; }
    public TypeScriptDiagnostic[] Errors { get; set; }
    public Map<WatchDirectoryFlags> WildcardDirectories { get; set; }
    public bool CompileOnSave { get; set; }
}

public sealed class ExpandResult
{
    public string[] FileNames { get; set; }
    public Map<WatchDirectoryFlags> WildcardDirectories { get; set; }
}

public class CommandLineOptionBase
{
    public string Name { get; set; }
    //  "string" | "number" | "boolean" | "object" | "list" | Map<number | string>
    public object Type { get; set; }
    public bool IsFilePath { get; set; }
    public string ShortName { get; set; }
    public DiagnosticMessage Description { get; set; }
    public DiagnosticMessage ParamType { get; set; }
    public bool IsTsConfigOnly { get; set; }
    public bool IsCommandLineOnly { get; set; }
    public bool ShowInSimplifiedHelpView { get; set; }
    public DiagnosticMessage Category { get; set; }
}

public sealed class CommandLineOptionOfPrimitiveType : CommandLineOptionBase
{
}

public sealed class CommandLineOptionOfCustomType : CommandLineOptionBase
{
}

public sealed class TsConfigOnlyOption : CommandLineOptionBase
{
}

public sealed class CommandLineOptionOfListType : CommandLineOptionBase
{
    public CommandLineOptionBase Element { get; set; }
}

public class ModuleResolutionHost
{
}

public class ResolvedModule
{
    public string ResolvedFileName { get; set; }
    public bool IsExternalLibraryImport { get; set; }
}

public sealed class ResolvedModuleFull : ResolvedModule
{
    public Extension Extension { get; set; }
}

public sealed class ResolvedModuleWithFailedLookupLocations
{
    public ResolvedModule ResolvedModule { get; set; }
    public string[] FailedLookupLocations { get; set; }
}

public sealed class ResolvedTypeReferenceDirective
{
    public bool Primary { get; set; }
    public string ResolvedFileName { get; set; }
}

public sealed class ResolvedTypeReferenceDirectiveWithFailedLookupLocations
{
    public ResolvedTypeReferenceDirective ResolvedTypeReferenceDirective { get; set; }
    public string[] FailedLookupLocations { get; set; }
}

public sealed class CompilerHost : ModuleResolutionHost
{
    public WriteFileCallback WriteFile { get; set; }
}

public sealed class EmitNode
{
    public Node[] AnnotatedNodes { get; set; }
    public EmitFlags Flags { get; set; }
    public SynthesizedComment[] LeadingComments { get; set; }
    public SynthesizedComment[] TrailingComments { get; set; }
    public TextRange CommentRange { get; set; }
    public TextRange SourceMapRange { get; set; }
    public TextRange[] TokenSourceMapRanges { get; set; }
    public int ConstantValue { get; set; }
    public Identifier ExternalHelpersModuleName { get; set; }
    public EmitHelper[] Helpers { get; set; }
}

public sealed class EmitHelper
{
    public string Name { get; set; }
    public bool Scoped { get; set; }
    public string Text { get; set; }
    public int Priority { get; set; }
}

public sealed class EmitHost : ScriptReferenceHost
{
    public WriteFileCallback WriteFile { get; set; }
}

public sealed class TransformationContext
{
    // onSubstituteNode { get; set; }
    // onEmitNode { get; set; }
}

public sealed class TransformationResult<T>
{
    public T[] Transformed { get; set; }
    public TypeScriptDiagnostic[] Diagnostics { get; set; }
}

public sealed class Printer
{
}

public sealed class PrintHandlers
{
    // onEmitSourceMapOfNode { get; set; }
    // onEmitSourceMapOfToken { get; set; }
    // onEmitSourceMapOfPosition { get; set; }
    // onEmitHelpers { get; set; }
    // onSetSourceFile { get; set; }
    // onBeforeEmitNodeArray { get; set; }
    // onAfterEmitNodeArray { get; set; }
}

public sealed class PrinterOptions
{
    public ScriptTarget Target { get; set; }
    public bool RemoveComments { get; set; }
    public NewLineKind NewLine { get; set; }
    public bool SourceMap { get; set; }
    public bool InlineSourceMap { get; set; }
    public bool ExtendedDiagnostics { get; set; }
}

public sealed class EmitTextWriter
{
}

public sealed class TextSpan
{
    public int Start { get; set; }
    public int Length { get; set; }
}

public sealed class TextChangeRange
{
    public TextSpan Span { get; set; } = default!;
    public int NewLength { get; set; }
}

public sealed class DiagnosticCollection
{
}

public sealed class SyntaxList : Node
{
    public SyntaxList() => Kind = TypeScriptSyntaxKind.SyntaxList;
}

public sealed class MissingNode : Identifier
{
}
