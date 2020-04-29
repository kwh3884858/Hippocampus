using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StarPlatinum.StoryCompile
{
    /// <summary>
    /// Translate content to token
    /// </summary>
    public sealed class StoryCompiler
    {

        public enum CompilerState
        {
            Error,
            StateString,

            StateInstructor,        //key word
            StateIdentity,          //variable

            StateStartBracket,
            StateStopBracket,
            StateSlash,
            StateOp,
        };

        public enum TokenType
        {
            TokenNone,

            TokenInstructor,         //key word
            TokenIdentity,           //variable

            TokenContent,

            TokenOpStartBracket,
            TokenOpStopBracket,
            TokenOpColon,
            TokenOpSlash,

        };
        public class Token
        {
            public TokenType m_tokeType;
            public string m_content;

            public Token()
            {
                m_tokeType = TokenType.TokenNone;
            }

            public Token(Token token)
            {
                m_tokeType = token.m_tokeType;
                m_content = token.m_content;
            }
        };

        public  StoryCompiler()
        {
            m_tokens = new List<Token>();
        }

        public List<Token> Compile(string word)
        {
            Lexer(word);

            return Output();
        }


        void Lexer(string word)
        {
            Clear();

            StringBuilder tmp = new StringBuilder();

            int length = word.Length;

            for (int j = 0; j < length; j++)
            {
                switch (word[j])
                {
                    case '<':

                        SwitchCompilerState(CompilerState.StateStartBracket);

                        break;

                    case '>':

                        SwitchCompilerState(CompilerState.StateStopBracket);
                        break;

                    case '/':
                        SwitchCompilerState(CompilerState.StateSlash);
                        break;

                    case ':':
                        SwitchCompilerState(CompilerState.StateOp);
                        break;

                    default:
                        SwitchCompilerState(CompilerState.StateString);
                        break;


                }



                if (m_currentState == m_lastState)
                {
                    //copy string

                    tmp.Append(word[j]);

                    continue;
                }

                CreateToken(ref tmp, m_lastState);

                m_lastState = m_currentState;

                //copy string

                tmp.Append(word[j]);

            }


            CreateToken(ref tmp, m_lastState);
        }


        void CreateToken(ref StringBuilder aString, CompilerState lastState)
        {
            if (aString.Length == 0)
            {
                return;
            }

            switch (lastState)
            {
                case CompilerState.Error:
                    break;
                case CompilerState.StateString:
                    {
                        Token token = CreateTokenByString(ref aString, TokenType.TokenContent);

                        break;
                    }
                case CompilerState.StateInstructor:
                    {
                        if (m_currentState == CompilerState.StateOp)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenInstructor);

                        }
                        else if (m_currentState == CompilerState.StateStopBracket)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenInstructor);
                        }

                        break;
                    }
                case CompilerState.StateIdentity:
                    {
                        Token token = CreateTokenByString(ref aString, TokenType.TokenIdentity);

                        break;
                    }

                case CompilerState.StateStartBracket:
                    {
                        if (m_currentState == CompilerState.StateInstructor)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenOpStartBracket);
                        }
                        else if (m_currentState == CompilerState.StateSlash)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenOpStartBracket);

                        }


                        break;
                    }
                case CompilerState.StateStopBracket:
                    {
                        Token token = CreateTokenByString(ref aString, TokenType.TokenOpStopBracket);

                        break;
                    }
                case CompilerState.StateOp:
                    {
                        if (m_currentState == CompilerState.StateInstructor)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenOpColon);
                        }
                        else if (m_currentState == CompilerState.StateStartBracket)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenOpColon);
                        }
                        else if (m_currentState == CompilerState.StateIdentity)
                        {
                            Token token = CreateTokenByString(ref aString, TokenType.TokenOpColon);
                        }
                        break;
                    }

                case CompilerState.StateSlash:
                    if (m_currentState == CompilerState.StateInstructor)
                    {
                        Token token = CreateTokenByString(ref aString, TokenType.TokenOpSlash);

                    }

                    break;
                default:
                    break;
            }
        }



        void SwitchCompilerState(CompilerState nextState)
        {
            switch (nextState)
            {
                case CompilerState.Error:

                    break;


                case CompilerState.StateString:
                    if (m_currentState == CompilerState.StateStopBracket)
                    {
                        m_currentState = nextState;
                    }
                    else if (m_currentState == CompilerState.StateStartBracket)
                    {
                        m_currentState = CompilerState.StateInstructor;
                    }
                    else if (m_currentState == CompilerState.StateOp)
                    {
                        m_currentState = CompilerState.StateIdentity;
                    }
                    else if (m_currentState == CompilerState.StateSlash)
                    {
                        m_currentState = CompilerState.StateInstructor;
                    }


                    break;

                case CompilerState.StateInstructor:

                    if (m_currentState == CompilerState.StateStartBracket)
                    {
                        m_currentState = nextState;
                    }
                    break;


                case CompilerState.StateIdentity:

                    if (m_currentState == CompilerState.StateOp)
                    {
                        m_currentState = nextState;
                    }

                    break;

                case CompilerState.StateStartBracket:
                    if (m_currentState == CompilerState.StateString)
                    {
                        m_currentState = nextState;
                    }
                    if (m_currentState == CompilerState.StateStopBracket)
                    {
                        m_currentState = nextState;
                    }

                    break;

                case CompilerState.StateStopBracket:
                    if (m_currentState == CompilerState.StateIdentity)
                    {
                        m_currentState = nextState;
                    }

                    if (m_currentState == CompilerState.StateInstructor)
                    {
                        m_currentState = nextState;
                    }

                    break;

                case CompilerState.StateOp:
                    if (m_currentState == CompilerState.StateInstructor)
                    {
                        m_currentState = nextState;
                    }
                    if (m_currentState == CompilerState.StateStartBracket)
                    {
                        m_currentState = nextState;
                    }

                    break;


                case CompilerState.StateSlash:
                    if (m_currentState == CompilerState.StateStartBracket)
                    {
                        m_currentState = nextState;
                    }

                    break;
                default:
                    break;
            }

            //m_lastState = m_state;
        }



        List<Token> Output()
        {
            List<Token> output = new List<Token>(m_tokens);

            Clear();

            return output;
        }

        Token CreateTokenByString(ref StringBuilder stringBuilder, TokenType tokenType)
        {
            Token token = new Token();

            token.m_content = stringBuilder.ToString();
            stringBuilder.Clear();
            token.m_tokeType = tokenType;

            AddToken(token);

            return token;
        }

        void AddToken(Token token)
        {
            m_tokens.Add(token);
        }

        void Clear()
        {
            m_tokens.Clear();
            m_currentState = CompilerState.StateString;
            m_lastState = CompilerState.StateString;
        }

        List<Token> m_tokens;
        CompilerState m_currentState;
        CompilerState m_lastState;


    }
}