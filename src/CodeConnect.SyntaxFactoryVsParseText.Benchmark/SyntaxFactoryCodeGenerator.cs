using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeConnect.SyntaxFactoryVsParseText.Benchmark
{
    public class SyntaxFactoryCodeGenerator : CodeGenerator
    {
        IEnumerable<string> _methodNames;
        bool _complexMethods;

        public override string ToString()
        {
            return "SyntaxFactory";
        }

        public override TypeDeclarationSyntax GenerateType(int methodCalls, bool complexMethods)
        {
            _methodNames = getIdentifierNames(methodCalls).ToList();
            _complexMethods = complexMethods;

            return
                SyntaxFactory.ClassDeclaration(
                    SyntaxFactory.Identifier(
                        SyntaxFactory.TriviaList(),
                        @"DemoClass",
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space)))
                .WithModifiers(
                    SyntaxFactory.TokenList(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Whitespace(
                                    @"    ")),
                            SyntaxKind.PublicKeyword,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Space))))
                .WithKeyword(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(),
                        SyntaxKind.ClassKeyword,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Space)))
                .WithBaseList(
                    SyntaxFactory.BaseList(
                        SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(
                            SyntaxFactory.SimpleBaseType(
                                SyntaxFactory.IdentifierName(
                                    SyntaxFactory.Identifier(
                                        SyntaxFactory.TriviaList(),
                                        @"IDemo",
                                        SyntaxFactory.TriviaList(
                                            SyntaxFactory.LineFeed))))))
                    .WithColonToken(
                        SyntaxFactory.Token(
                            SyntaxFactory.TriviaList(),
                            SyntaxKind.ColonToken,
                            SyntaxFactory.TriviaList(
                                SyntaxFactory.Space))))
                .WithOpenBraceToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Whitespace(
                                @"    ")),
                        SyntaxKind.OpenBraceToken,
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.LineFeed)))
                .WithMembers(
                    SyntaxFactory.List<MemberDeclarationSyntax>(
                        GetAllMembers())) // <--
                .WithCloseBraceToken(
                    SyntaxFactory.Token(
                        SyntaxFactory.TriviaList(
                            SyntaxFactory.Whitespace(
                                @"    ")),
                        SyntaxKind.CloseBraceToken,
                        SyntaxFactory.TriviaList()));
        }

        private IEnumerable<MemberDeclarationSyntax> GetAllMembers()
        {
            var runMethod = GetRunMethod();
            var otherMethods = GetMethods();

            var allMembers = new List<MemberDeclarationSyntax>();
            allMembers.Add(runMethod);
            allMembers.AddRange(otherMethods);
            return allMembers;
        }

        private MemberDeclarationSyntax GetRunMethod()
        {
            return
                    SyntaxFactory.MethodDeclaration(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VoidKeyword,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))),
                        SyntaxFactory.Identifier(
                            @"Run"))
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.PublicKeyword,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))))
                    .WithParameterList(
                        SyntaxFactory.ParameterList()
                        .WithOpenParenToken(
                            SyntaxFactory.Token(
                                SyntaxKind.OpenParenToken))
                        .WithCloseParenToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.CloseParenToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed))))
                    .WithBody(
                        SyntaxFactory.Block(
                            SyntaxFactory.List<StatementSyntax>(
                                getMethodInvocations())) // <--
                        .WithOpenBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.OpenBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed)))
                        .WithCloseBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.CloseBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed))));
        }

        /// <summary>
        /// Gets the body of the invocation method, for example
        ///  {
        ///     var aliveIntermediate = new HelloTest();
        ///     {
        ///         aliveIntermediate.TestFixtureSetUp();
        ///         aliveIntermediate.SetUp();
        ///         aliveIntermediate.TestMethod1();
        ///         aliveIntermediate.TearDown();
        ///         aliveIntermediate.TestFixtureTearDown();
        ///     }
        ///  }
        /// </summary>
        /// <returns></returns>
        private IEnumerable<StatementSyntax> getMethodInvocations()
        {
            foreach (var methodName in _methodNames)
            {
                yield return
                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.IdentifierName(
                                                SyntaxFactory.Identifier(
                                                    SyntaxFactory.TriviaList(
                                                        SyntaxFactory.Whitespace(
                                                            @"            ")),
                                                    methodName, // <--
                                                    SyntaxFactory.TriviaList())))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList()
                                            .WithOpenParenToken(
                                                SyntaxFactory.Token(
                                                    SyntaxKind.OpenParenToken))
                                            .WithCloseParenToken(
                                                SyntaxFactory.Token(
                                                    SyntaxKind.CloseParenToken))))
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.SemicolonToken,
                                            SyntaxFactory.TriviaList(
                                                SyntaxFactory.LineFeed)));
            }
        }

        private IEnumerable<MethodDeclarationSyntax> GetMethods()
        {
            foreach (var methodName in _methodNames)
            {
                yield return
                    SyntaxFactory.MethodDeclaration(
                        SyntaxFactory.PredefinedType(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.VoidKeyword,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))),
                        SyntaxFactory.Identifier(
                            methodName)) // <--
                    .WithModifiers(
                        SyntaxFactory.TokenList(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    new[]{
                                        SyntaxFactory.LineFeed,
                                        SyntaxFactory.Whitespace(
                                            @"        ")}),
                                SyntaxKind.PrivateKeyword,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Space))))
                    .WithParameterList(
                        SyntaxFactory.ParameterList()
                        .WithOpenParenToken(
                            SyntaxFactory.Token(
                                SyntaxKind.OpenParenToken))
                        .WithCloseParenToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(),
                                SyntaxKind.CloseParenToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed))))
                    .WithBody(
                        getMethodBody(methodName)); // <--
            }
        }

        private BlockSyntax getMethodBody(string methodName)
        {
            if (!_complexMethods)
            {
                // Method's body is empty
                return
                        SyntaxFactory.Block()
                        .WithOpenBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.OpenBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed)))
                        .WithCloseBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.CloseBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed)));
            }
            else
            {
                // We're making a few more calls in the method's body
                return
                        SyntaxFactory.Block(
                            SyntaxFactory.List<StatementSyntax>(
                                new StatementSyntax[]{
                                    SyntaxFactory.LocalDeclarationStatement(
                                        SyntaxFactory.VariableDeclaration(
                                            SyntaxFactory.IdentifierName(
                                                SyntaxFactory.Identifier(
                                                    SyntaxFactory.TriviaList(
                                                        SyntaxFactory.Whitespace(
                                                            @"            ")),
                                                    @"var",
                                                    SyntaxFactory.TriviaList(
                                                        SyntaxFactory.Space))))
                                        .WithVariables(
                                            SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                                SyntaxFactory.VariableDeclarator(
                                                    SyntaxFactory.Identifier(
                                                        SyntaxFactory.TriviaList(),
                                                        @"myNumber",
                                                        SyntaxFactory.TriviaList(
                                                            SyntaxFactory.Space)))
                                                .WithInitializer(
                                                    SyntaxFactory.EqualsValueClause(
                                                        SyntaxFactory.InvocationExpression(
                                                            SyntaxFactory.MemberAccessExpression(
                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                SyntaxFactory.IdentifierName(
                                                                    @"Int32"),
                                                                SyntaxFactory.IdentifierName(
                                                                    @"Parse"))
                                                            .WithOperatorToken(
                                                                SyntaxFactory.Token(
                                                                    SyntaxKind.DotToken)))
                                                        .WithArgumentList(
                                                            SyntaxFactory.ArgumentList(
                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                    SyntaxFactory.Argument(
                                                                        SyntaxFactory.InvocationExpression(
                                                                            SyntaxFactory.MemberAccessExpression(
                                                                                SyntaxKind.SimpleMemberAccessExpression,
                                                                                SyntaxFactory.LiteralExpression(
                                                                                    SyntaxKind.StringLiteralExpression,
                                                                                    SyntaxFactory.Literal(
                                                                                        SyntaxFactory.TriviaList(),
                                                                                        $@"""{methodName}""",
                                                                                        $@"""{methodName}""",
                                                                                        SyntaxFactory.TriviaList())),
                                                                                SyntaxFactory.IdentifierName(
                                                                                    @"Substring"))
                                                                            .WithOperatorToken(
                                                                                SyntaxFactory.Token(
                                                                                    SyntaxKind.DotToken)))
                                                                        .WithArgumentList(
                                                                            SyntaxFactory.ArgumentList(
                                                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                                                    SyntaxFactory.Argument(
                                                                                        SyntaxFactory.LiteralExpression(
                                                                                            SyntaxKind.NumericLiteralExpression,
                                                                                            SyntaxFactory.Literal(
                                                                                                SyntaxFactory.TriviaList(),
                                                                                                @"6",
                                                                                                6,
                                                                                                SyntaxFactory.TriviaList())))))
                                                                            .WithOpenParenToken(
                                                                                SyntaxFactory.Token(
                                                                                    SyntaxKind.OpenParenToken))
                                                                            .WithCloseParenToken(
                                                                                SyntaxFactory.Token(
                                                                                    SyntaxKind.CloseParenToken))))))
                                                            .WithOpenParenToken(
                                                                SyntaxFactory.Token(
                                                                    SyntaxKind.OpenParenToken))
                                                            .WithCloseParenToken(
                                                                SyntaxFactory.Token(
                                                                    SyntaxKind.CloseParenToken))))
                                                    .WithEqualsToken(
                                                        SyntaxFactory.Token(
                                                            SyntaxFactory.TriviaList(),
                                                            SyntaxKind.EqualsToken,
                                                            SyntaxFactory.TriviaList(
                                                                SyntaxFactory.Space)))))))
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.SemicolonToken,
                                            SyntaxFactory.TriviaList(
                                                SyntaxFactory.LineFeed))),
                                    SyntaxFactory.ExpressionStatement(
                                        SyntaxFactory.InvocationExpression(
                                            SyntaxFactory.MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                SyntaxFactory.IdentifierName(
                                                    SyntaxFactory.Identifier(
                                                        SyntaxFactory.TriviaList(
                                                            SyntaxFactory.Whitespace(
                                                                @"            ")),
                                                        @"Console",
                                                        SyntaxFactory.TriviaList())),
                                                SyntaxFactory.IdentifierName(
                                                    @"WriteLine"))
                                            .WithOperatorToken(
                                                SyntaxFactory.Token(
                                                    SyntaxKind.DotToken)))
                                        .WithArgumentList(
                                            SyntaxFactory.ArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                                    SyntaxFactory.Argument(
                                                        SyntaxFactory.BinaryExpression(
                                                            SyntaxKind.AddExpression,
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.StringLiteralExpression,
                                                                SyntaxFactory.Literal(
                                                                    SyntaxFactory.TriviaList(),
                                                                    $@"""{methodName} says hi to """,
                                                                    $@"""{methodName} says hi to """,
                                                                    SyntaxFactory.TriviaList(
                                                                        SyntaxFactory.Space))),
                                                            SyntaxFactory.ParenthesizedExpression(
                                                                SyntaxFactory.BinaryExpression(
                                                                    SyntaxKind.AddExpression,
                                                                    SyntaxFactory.IdentifierName(
                                                                        SyntaxFactory.Identifier(
                                                                            SyntaxFactory.TriviaList(),
                                                                            @"myNumber",
                                                                            SyntaxFactory.TriviaList(
                                                                                SyntaxFactory.Space))),
                                                                    SyntaxFactory.LiteralExpression(
                                                                        SyntaxKind.NumericLiteralExpression,
                                                                        SyntaxFactory.Literal(
                                                                            SyntaxFactory.TriviaList(),
                                                                            @"1",
                                                                            1,
                                                                            SyntaxFactory.TriviaList())))
                                                                .WithOperatorToken(
                                                                    SyntaxFactory.Token(
                                                                        SyntaxFactory.TriviaList(),
                                                                        SyntaxKind.PlusToken,
                                                                        SyntaxFactory.TriviaList(
                                                                            SyntaxFactory.Space))))
                                                            .WithOpenParenToken(
                                                                SyntaxFactory.Token(
                                                                    SyntaxKind.OpenParenToken))
                                                            .WithCloseParenToken(
                                                                SyntaxFactory.Token(
                                                                    SyntaxKind.CloseParenToken)))
                                                        .WithOperatorToken(
                                                            SyntaxFactory.Token(
                                                                SyntaxFactory.TriviaList(),
                                                                SyntaxKind.PlusToken,
                                                                SyntaxFactory.TriviaList(
                                                                    SyntaxFactory.Space))))))
                                            .WithOpenParenToken(
                                                SyntaxFactory.Token(
                                                    SyntaxKind.OpenParenToken))
                                            .WithCloseParenToken(
                                                SyntaxFactory.Token(
                                                    SyntaxKind.CloseParenToken))))
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(
                                            SyntaxFactory.TriviaList(),
                                            SyntaxKind.SemicolonToken,
                                            SyntaxFactory.TriviaList(
                                                SyntaxFactory.LineFeed)))}))
                        .WithOpenBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.OpenBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed)))
                        .WithCloseBraceToken(
                            SyntaxFactory.Token(
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.Whitespace(
                                        @"        ")),
                                SyntaxKind.CloseBraceToken,
                                SyntaxFactory.TriviaList(
                                    SyntaxFactory.LineFeed)));
            }
        }
    }
}
