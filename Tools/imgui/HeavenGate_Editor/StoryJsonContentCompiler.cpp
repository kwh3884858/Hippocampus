#include "StoryJsonContentCompiler.h"

#include "StoryJson.h"
#include "StoryTableManager.h"

namespace HeavenGateEditor {


    void StoryJsonContentCompiler::Compile(StoryJson* const storyJson)
    {
        for (int i = 0; i < storyJson->Size(); i++)
        {
            const StoryNode* const node = storyJson->GetNode(i);

            if (node->m_nodeType != NodeType::Word) continue;

            const StoryWord* const word = static_cast<const StoryWord* const>(node);

            CompileEach(word);

        }
    }

    void StoryJsonContentCompiler::CompileEach(const StoryWord* const word)
    {
        Lexer(word);
        Parser();
    }


    void StoryJsonContentCompiler::Lexer(const StoryWord* const word)
    {
        m_state = CompilerState::StateString;
        char tmp[MAX_CONTENT];
        memset(tmp, '\0', sizeof(tmp));


        int length = strlen(word->m_content);

        for (int j = 0; j < length; j++)
        {
            switch (word->m_content[j])
            {
            case '<':
            {
                SwitchCompilerState(CompilerState::StateStartBracket);

                break;
            }
            case '>':
            {
                SwitchCompilerState(CompilerState::StateStopBracket);
                break;
            }
            case ':':
            {
                SwitchCompilerState(CompilerState::StateOp);
                break;
            }

            default: {
                SwitchCompilerState(CompilerState::StateString);
                break;
            }

            }
            if (m_state == m_lastState)
            {
                sprintf(tmp, "%s", word->m_content[j]);
                continue;
            }

            switch (m_lastState)
            {
            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::Error:
                break;
            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateString:
            {
                Token* token = CreateTokenByString(tmp, TokenType::TokenContent);

                break;
            }
            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateInstructor: {
                if (m_state == CompilerState::StateOp)
                {
                    Token* token = CreateTokenByString(tmp, TokenType::TokenInstructor);

                }
                else if (m_state == CompilerState::StateInstructor)
                {
                    Token* token = CreateTokenByString(tmp, TokenType::TokenCloseLabel);

                }

                break;
            }
            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateIdent: {
                Token* token = CreateTokenByString(tmp, TokenType::TokenIdnet);

                break;
            }

            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStartBracket: {

                Token* token = CreateTokenByString(tmp, TokenType::TokenOpBracketLeft);

                break;
            }
            case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStopBracket:
            {
                Token* token = CreateTokenByString(tmp, TokenType::TokenOpBracketRight);

                break;
            }
            case  HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateOp:
            {
                Token* token = CreateTokenByString(tmp, TokenType::TokenOpColon);

                break;
            }
            default:
                break;
            }
        }

    }




    void StoryJsonContentCompiler::SwitchCompilerState(CompilerState state)
    {
        switch (state)
        {
        case CompilerState::Error:
        {
            break;
        }

        case CompilerState::StateString: {
            if (m_state == CompilerState::StateStopBracket)
            {
                m_state = state;
            }
            else if (m_state == CompilerState::StateStartBracket)
            {
                m_state = CompilerState::StateInstructor;
            }
            else if (m_state == CompilerState::StateOp)
            {
                m_state = CompilerState::StateIdent;
            }

            break;
        }
        case CompilerState::StateInstructor:
        {
            if (m_state == CompilerState::StateStartBracket)
            {
                m_state = state;
            }
            break;

        }
        case CompilerState::StateIdent:
        {
            if (m_state == CompilerState::StateOp)
            {
                m_state = state;
            }

            break;
        }
        case CompilerState::StateStartBracket: {
            if (m_state == CompilerState::StateString)
            {
                m_state = state;
            }

            break;
        }
        case CompilerState::StateStopBracket: {
            if (m_state == CompilerState::StateIdent)
            {
                m_state = state;
            }

            if (m_state == CompilerState::StateInstructor)
            {
                m_state = state;
            }

            break;
        }
        case CompilerState::StateOp: {
            if (m_state == CompilerState::StateInstructor)
            {
                m_state = state;
            }

            break;
        }
        default:
            break;
        }

        m_lastState = m_state;
    }


    void StoryJsonContentCompiler::Parser()
    {
        for (int i = 0; i < m_tokens.size(); i++)
        {
            if (m_tokens[i]->m_tokeType == TokenType::TokenIdnet)
            {

            }
        }
    }

    HeavenGateEditor::StoryJsonContentCompiler::Token*const StoryJsonContentCompiler::CreateTokenByString(char*const aString, TokenType tokenType)
    {
        Token* token = new Token;
        strcpy(token->m_content, aString);
        token->m_tokeType = tokenType;
        AddToken(token);

        memset(aString, '\0', MAX_CONTENT);
        return token;
    }

    void StoryJsonContentCompiler::AddToken(Token*const token)
    {
        m_tokens.push_back(token);
    }

}
