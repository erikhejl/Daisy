namespace Ancestry.Daisy.Language
{
    using Ancestry.Daisy.Language.AST;
    using Ancestry.Daisy.Utils;

    public class DaisyParser
    {
        private readonly LookAheadStream<Token> tokenStream;

        public DaisyParser(LookAheadStream<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
        }

        public DaisyAst Parse()
        {
            var root = tokenStream.MoveNext() 
                ? ParseExpression() 
                : null;
            return new DaisyAst(root);
        }

        private IDaisyAstNode ParseExpression()
        {
            var statement = ParseUnaryExpr();
            var endLoop = false;
            while(!endLoop)
            {
                switch (tokenStream.Current.Kind)
                {
                    case TokenKind.Statement:
                    case TokenKind.And:
                    case TokenKind.Not:
                        statement = ParseAnd(statement);
                        break;
                    case TokenKind.Or:
                        statement = ParseOr(statement);
                        break;
                    case TokenKind.EOF:
                    case TokenKind.EndGroup:
                        endLoop = true;
                        break;
                    default:
                        throw new UnexpectedTokenException(tokenStream.Current, new[]
                            {
                                TokenKind.Statement, TokenKind.And, TokenKind.Or,
                                TokenKind.EOF, TokenKind.StartGroup, 
                            });
                }
            }
            return statement;
        }

        private AndOperator ParseAnd(IDaisyAstNode left)
        {
            if(tokenStream.Current.Kind == TokenKind.And) MoveNextGuaranteed();
            var right = ParseUnaryExpr();
            return new AndOperator(left, right);
        }

        private IDaisyAstNode ParseUnaryExpr()
        {
            if(tokenStream.Current.Kind == TokenKind.Not)
            {
                MoveNextGuaranteed();
                var pred = ParsePredicate();
                return new NotOperator(pred);
            }
            return ParsePredicate();
        }


        private GroupOperator ParseGroup(Statement head)
        {
            Consume(TokenKind.StartGroup);
            var groupInner = ParseExpression();
            Consume(TokenKind.EndGroup);
            return new GroupOperator(head.With(x => head.Text),
                groupInner);
        }

        private IDaisyAstNode ParsePredicate()
        {
            if(tokenStream.Current.Kind == TokenKind.StartGroup) //Anonymous group
            {
                return ParseGroup(null);
            }
            var statement =  ParseStatement();
            if(tokenStream.Current.Kind == TokenKind.StartGroup) //Named group
            {
                return ParseGroup(statement);
            }
            return statement;
        }

        private OrOperator ParseOr(IDaisyAstNode left)
        {
            Consume(TokenKind.Or);
            var right = ParseUnaryExpr();
            return new OrOperator(left, right);
        }

        private Statement ParseStatement()
        {
            AssertHasType(TokenKind.Statement);
            var statement =  new Statement(tokenStream.Current.Value);
            tokenStream.MoveNext();
            return statement;
        }

        private void AssertHasType(TokenKind kind)
        {
            if (tokenStream.Current.Kind != kind)
                throw new UnexpectedTokenException(tokenStream.Current, kind) { };
        }

        private void Consume(TokenKind kind)
        {
            AssertHasType(kind);
            MoveNextGuaranteed();
        }

        private void MoveNextGuaranteed()
        {
            if(!tokenStream.MoveNext()) { throw new UnexpectedEndOfStreamException(); }
        }

        public static DaisyAst Parse(string code)
        {
            var llstream = new LookAheadStream<Token>(new Lexer(code.ToStream()).Lex());
            var parser = new DaisyParser(llstream);
            return parser.Parse();
        }
    }
}
