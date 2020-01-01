#include "StoryJsonContentCompiler.h"

#include "StoryJson.h"
#include "StoryTableManager.h"

#include "StoryTable.h"

namespace HeavenGateEditor {


    void StoryJsonContentCompiler::Compile(StoryJson* const storyJson)
    {
        for (int i = 0; i < storyJson->Size(); i++)
        {
            StoryNode* const node = storyJson->GetNode(i);

            if (node->m_nodeType != NodeType::Word) continue;

            StoryWord* const word = static_cast<StoryWord* const>(node);

            CompileEach(word);

        }
    }


    vector<StoryJsonContentCompiler::Token*> StoryJsonContentCompiler::CompileToTokens(StoryWord* const storyWord)
    {
        Lexer(storyWord);
        Parser();

        vector<StoryJsonContentCompiler::Token*> copyTokens;
        for (auto iter = m_tokens.begin(); iter != m_tokens.end(); iter++)
        {
            Token* copyToken = new Token(**iter);
            copyTokens.push_back(copyToken);
        }

        return m_tokens;
    }


    void StoryJsonContentCompiler::CompileEach(StoryWord* const word)
    {
        Lexer(word);
        Parser();

        Output(word);
    }


    void StoryJsonContentCompiler::Lexer(const StoryWord* const word)
    {
        Clear();

        char tmp[MAX_CONTENT];
        memset(tmp, '\0', sizeof(tmp));


        int length = strlen(word->m_content);

        for (int j = 0; j < length; j++)
        {
            switch (word->m_content[j])
            {
            case '<':

                SwitchCompilerState(CompilerState::StateStartBracket);

                break;

            case '>':

                SwitchCompilerState(CompilerState::StateStopBracket);
                break;

            case '/': case ':':
                SwitchCompilerState(CompilerState::StateOp);
                break;

            default:
                SwitchCompilerState(CompilerState::StateString);
                break;


            }



            if (m_state == m_lastState)
            {
                //copy char
                char c[2] = "a";
                c[0] = word->m_content[j];
                strcat(tmp, c);

                continue;
            }

            CreateToken(tmp, m_lastState);

            m_lastState = m_state;

            //copy char
            char c[2] = "a";
            c[0] = word->m_content[j];
            strcat(tmp, c);

        }


        CreateToken(tmp, m_lastState);
    }


    void StoryJsonContentCompiler::CreateToken(char*const aString, CompilerState lastState)
    {
        if (strlen(aString) == 0)
        {
            return;
        }

        switch (lastState)
        {
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::Error:
            break;
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateString:
        {
            Token* token = CreateTokenByString(aString, TokenType::TokenContent);

            break;
        }
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateInstructor: {
            if (m_state == CompilerState::StateOp)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenInstructor);

            }
            else if (m_state == CompilerState::StateInstructor)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenCloseLabel);

            }

            break;
        }
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateIdent: {
            Token* token = CreateTokenByString(aString, TokenType::TokenIdnet);

            break;
        }

        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStartBracket: {

            Token* token = CreateTokenByString(aString, TokenType::TokenOpBracketLeft);

            break;
        }
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStopBracket:
        {
            Token* token = CreateTokenByString(aString, TokenType::TokenOpBracketRight);

            break;
        }
        case  HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateOp:
        {
            if (m_state == CompilerState::StateInstructor)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            else if (m_state == CompilerState::StateStartBracket)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            else if (m_state == CompilerState::StateIdent)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            break;
        }
        default:
            break;
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
            if (m_state == CompilerState::StateStartBracket)
            {
                m_state = state;
            }

            break;
        }
        default:
            break;
        }

        //m_lastState = m_state;
    }


    void StoryJsonContentCompiler::Parser()
    {
        TableType currentTableType = TableType::None;

        for (int i = 0; i < m_tokens.size(); i++)
        {
            Token* token = m_tokens[i];
            if (token->m_tokeType == TokenType::TokenInstructor)
            {

                if (strcmp(token->m_content, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0)
                {
                    currentTableType = TableType::Font_Size;
                }
                if (strcmp(token->m_content, colorTableString[(int)FontSizeTableLayout::Type]) == 0)
                {
                    currentTableType = TableType::Color;
                }
                if (strcmp(token->m_content, paintMoveTableString[(int)FontSizeTableLayout::Type]) == 0)
                {
                    currentTableType = TableType::Paint_Move;
                }
            }
            else if (token->m_tokeType == TokenType::TokenIdnet)
            {
                switch (currentTableType)
                {
                case HeavenGateEditor::TableType::None:
                    break;
                case HeavenGateEditor::TableType::Font_Size:
                {
                    const StoryTable<FONT_SIZE_MAX_COLUMN>* const fontSizeTable = StoryTableManager::Instance().GetFontSizeTable();
                    for (int i = 0; i < fontSizeTable->GetSize(); i++)
                    {
                        const StoryRow<FONT_SIZE_MAX_COLUMN>* const row = fontSizeTable->GetRow(i);
                        if (strcmp(row->Get(0), token->m_content) == 0)
                        {
                            strcpy(token->m_content, row->Get(1));
                        }
                    }

                    break;
                }
                case HeavenGateEditor::TableType::Color: {
                    const StoryTable<COLOR_MAX_COLUMN>* const colorTable = StoryTableManager::Instance().GetColorTable();
                    for (int i = 0; i < colorTable->GetSize(); i++)
                    {
                        const StoryRow<COLOR_MAX_COLUMN>* const row = colorTable->GetRow(i);
                        if (strcmp(row->Get(0), token->m_content) == 0)
                        {
                            strcpy(token->m_content, row->Get(1));
                        }
                    }
                    break;
                }
                                         
                default:
                    break;
                }

                currentTableType = TableType::None;
            }

        }
    }

    void StoryJsonContentCompiler::Output(StoryWord* const word)
    {
        memset(word->m_content, '\0', MAX_CONTENT);

        for (int i = 0; i < m_tokens.size(); i++)
        {
            strcat(word->m_content, m_tokens[i]->m_content);
        }

        Clear();
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

    void StoryJsonContentCompiler::Clear()
    {
        for (auto iter = m_tokens.begin(); iter != m_tokens.end(); iter++)
        {
            delete *iter;
            *iter = nullptr;
        }
        m_tokens.clear();
        m_state = CompilerState::StateString;
        m_lastState = CompilerState::StateString;
    }

}
