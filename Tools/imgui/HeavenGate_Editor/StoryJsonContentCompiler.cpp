#include "StoryJsonContentCompiler.h"
#include "HeavenGateEditorUtility.h"

#include "imgui.h"

#include "StoryJson.h"
#include "StoryJsonWordNode.h"
#include "StoryTableManager.h"

#include "StoryTable.h"
#include "deque"
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

            case '/':
                SwitchCompilerState(CompilerState::StateSlash);
                break;

            case ':':
                SwitchCompilerState(CompilerState::StateOp);
                break;

            default:
                SwitchCompilerState(CompilerState::StateString);
                break;
            }



            if (m_currentState == m_lastState)
            {
                //copy char
                char c[2] = "a";
                c[0] = word->m_content[j];
                strcat(tmp, c);

                continue;
            }

            CreateToken(tmp, m_lastState);

            m_lastState = m_currentState;

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
            if (m_currentState == CompilerState::StateOp)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenInstructor);

            }
            else if (m_currentState == CompilerState::StateStopBracket) {
                Token* token = CreateTokenByString(aString, TokenType::TokenInstructor);
            }

            break;
        }
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateIdentity: {
            Token* token = CreateTokenByString(aString, TokenType::TokenIdentity);

            break;
        }

        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStartBracket: {
            if (m_currentState == CompilerState::StateInstructor)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpStartBracket);
            }
            else if (m_currentState == CompilerState::StateSlash)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpStartBracket);

            }


            break;
        }
        case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStopBracket:
        {
            Token* token = CreateTokenByString(aString, TokenType::TokenOpStopBracket);

            break;
        }
        case  HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateOp:
        {
            if (m_currentState == CompilerState::StateInstructor)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            else if (m_currentState == CompilerState::StateStartBracket)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            else if (m_currentState == CompilerState::StateIdentity)
            {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpColon);
            }
            break;
        }

        case CompilerState::StateSlash:
            if (m_currentState == CompilerState::StateInstructor) {
                Token* token = CreateTokenByString(aString, TokenType::TokenOpSlash);

            }

            break;
        default:
            break;
        }
    }



    void StoryJsonContentCompiler::SwitchCompilerState(CompilerState nextState)
    {
        switch (nextState)
        {
        case CompilerState::Error:
            break;


        case CompilerState::StateString:
            if (m_currentState == CompilerState::StateStopBracket)
            {
                m_currentState = nextState;
            }
            else if (m_currentState == CompilerState::StateStartBracket)
            {
                m_currentState = CompilerState::StateInstructor;
            }
            else if (m_currentState == CompilerState::StateOp)
            {
                m_currentState = CompilerState::StateIdentity;
            }
            else if (m_currentState == CompilerState::StateSlash)
            {
                m_currentState = CompilerState::StateInstructor;
            }
            break;

        case CompilerState::StateInstructor:
            if (m_currentState == CompilerState::StateStartBracket)
            {
                m_currentState = nextState;
            }
            break;
        case CompilerState::StateIdentity:
            if (m_currentState == CompilerState::StateOp)
            {
                m_currentState = nextState;
            }
            break;
        case CompilerState::StateStartBracket:
            if (m_currentState == CompilerState::StateString)
            {
                m_currentState = nextState;
            }
            if (m_currentState == CompilerState::StateStopBracket)
            {
                m_currentState = nextState;
            }
            break;
        case CompilerState::StateStopBracket:
            if (m_currentState == CompilerState::StateIdentity)
            {
                m_currentState = nextState;
            }

            if (m_currentState == CompilerState::StateInstructor)
            {
                m_currentState = nextState;
            }
            break;
        case CompilerState::StateOp:
            if (m_currentState == CompilerState::StateInstructor)
            {
                m_currentState = nextState;
            }
            if (m_currentState == CompilerState::StateStartBracket)
            {
                m_currentState = nextState;
            }
            break;
        case CompilerState::StateSlash:
            if (m_currentState == CompilerState::StateStartBracket) {
                m_currentState = nextState;
            }

            break;
        default:
            break;
        }
    }


    void StoryJsonContentCompiler::Parser()
    {
        TableType currentState;
        std::deque<TableType> editorState;
        bool isReadCloseLabel;

        isReadCloseLabel = false;
        currentState = TableType::None;

        for (int i = 0; i < m_tokens.size(); i++)
        {
            Token* token = m_tokens[i];
            if (token->m_tokeType == TokenType::TokenInstructor)
            {
                if (isReadCloseLabel)
                {
                    // Is closing state
                    if (editorState.empty())
                    {
                        printf("Close Label is lack\n");
                        continue;
                    }

                    currentState = editorState.back();
                    editorState.pop_back();
                    isReadCloseLabel = false;

                    switch (currentState)
                    {
                    case HeavenGateEditor::TableType::None:
                        break;
                    case HeavenGateEditor::TableType::Font_Size:
                        break;
                    case HeavenGateEditor::TableType::Color:
                        break;
                    case HeavenGateEditor::TableType::Tips:
                        break;
                    case HeavenGateEditor::TableType::Paint_Move:
                        break;
                    case TableType::Pause:
                        break;
                    case TableType::Tachie:
                        break;
                        case TableType::Bgm:
                            break;
                        case  TableType::Effect:
                            break;
                    default:
                        break;
                    }
                }
                else
                {
                    // Is start state
                    if (strcmp(token->m_content, fontSizeTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Font_Size);
                    }
                    if (strcmp(token->m_content, colorTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Color);
                    }
                    if (strcmp(token->m_content, paintMoveTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Paint_Move);
                    }
                    if (strcmp(token->m_content, pauseTableString[(int)FontSizeTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Pause);
                    }
                    if (strcmp(token->m_content, tachieTableString[(int)TachieTableLayout::Type]) == 0)
                    {
                        editorState.push_back(TableType::Tachie);
                    }
                    if (strcmp(token->m_content, bgmTableString[(int)BgmTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Bgm);
                    }
                    if (strcmp(token->m_content, effectTableString[(int)EffectTableLayout::Type]) == 0) {
                        editorState.push_back(TableType::Effect);
                    }
                }
            }
            else if (token->m_tokeType == TokenType::TokenIdentity)
            {
                //Modify data from editor envirenment to runtime
                if (editorState.empty())
                {
                    printf("No Identity Exist. \n");
                    continue;
                }
                switch (editorState.back())
                {
                case HeavenGateEditor::TableType::None:
                    break;
                case HeavenGateEditor::TableType::Font_Size: {
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
                case HeavenGateEditor::TableType::Color:
                {
                    char colorAlias[MAX_COLUMNS_CONTENT_LENGTH];

                    StoryTable<COLOR_MAX_COLUMN>* colorTable = StoryTableManager::Instance().GetColorTable();
                    for (int i = 0; i < colorTable->GetSize(); i++)
                    {
                        const StoryRow<COLOR_MAX_COLUMN>* const row = colorTable->GetRow(i);
                        strcpy(colorAlias, colorTable->GetRow(i)->Get(0));
                        if (strcmp(colorAlias, token->m_content) == 0)
                        {
                            ImVec4 color(1.0f, 1.0f, 1.0f, 1.0f);
                            color = ImVec4(
                                atoi(colorTable->GetRow(i)->Get(1)),
                                atoi(colorTable->GetRow(i)->Get(2)),
                                atoi(colorTable->GetRow(i)->Get(3)),
                                atoi(colorTable->GetRow(i)->Get(4))
                            );
                            unsigned int unsignedColor = HeavenGateEditorUtility::ConvertRGBAToUnsignedInt(color);

                            sprintf(token->m_content, "%08x", unsignedColor);

                        }

                    }
                    break;
                }
                case HeavenGateEditor::TableType::Tips:
                        //Do Not Modify
                    break;
                case HeavenGateEditor::TableType::Paint_Move:
                    break;
                case HeavenGateEditor::TableType::Exhibit:
                        //Do Not Modify
                    break;
                case HeavenGateEditor::TableType::Pause:
                {
                    const StoryTable<PAUSE_MAX_COLUMN>* const pauseTable = StoryTableManager::Instance().GetPauseTable();
                    for (int i = 0; i < pauseTable->GetSize(); i++)
                    {
                        const StoryRow<PAUSE_MAX_COLUMN>* const row = pauseTable->GetRow(i);
                        if (strcmp(row->Get(0), token->m_content) == 0)
                        {
                            strcpy(token->m_content, row->Get(1));
                        }
                    }
                }
                        break;

                    case TableType::Bgm:
                    {
                        const StoryTable<BGM_MAX_COLUMN>* const bgmTable = StoryTableManager::Instance().GetBgmTable();
                        for (int i = 0; i < bgmTable->GetSize(); i++)
                        {
                            const StoryRow<BGM_MAX_COLUMN>* const row = bgmTable->GetRow(i);
                            if (strcmp(row->Get(0), token->m_content) == 0)
                            {
                                strcpy(token->m_content, row->Get(1));
                            }
                        }
                    }
                        break;

                    case TableType::Effect:
                    {
                        const StoryTable<EFFECT_MAX_COLUMN>* const effectTable = StoryTableManager::Instance().GetEffectTable();
                        for (int i = 0; i < effectTable->GetSize(); i++)
                        {
                            const StoryRow<EFFECT_MAX_COLUMN>* const row = effectTable->GetRow(i);
                            if (strcmp(row->Get(0), token->m_content) == 0)
                            {
                                strcpy(token->m_content, row->Get(1));
                            }
                        }
                    }
                        break;
                case HeavenGateEditor::TableType::Tachie:
                {
                    char tachieCommand[NUM_OF_TACHIE_COMMAND][MAX_COLUMNS_CONTENT_LENGTH];
                    IdOperator::ParseStringId<'+', MAX_COLUMNS_CONTENT_LENGTH, NUM_OF_TACHIE_COMMAND>(token->m_content, tachieCommand);

                    const StoryTable<TACHIE_MAX_COLUMN>* const tachieTable = StoryTableManager::Instance().GetTachieTable();
                    const StoryTable<TACHIE_POSITION_MAX_COLUMN>* const tachiePositionTable = StoryTableManager::Instance().GetTachiePositionTable();
                    for (int i = 0; i < tachieTable->GetSize(); i++)
                    {
                        const StoryRow<TACHIE_MAX_COLUMN>* const row = tachieTable->GetRow(i);
                        if (strcmp(row->Get(0), tachieCommand[0]) == 0)
                        {
                            strcpy(tachieCommand[0], row->Get(1));
                        }
                    }
                    for (int i = 0; i < tachiePositionTable->GetSize(); i++)
                    {
                        const StoryRow<TACHIE_POSITION_MAX_COLUMN>* const row = tachiePositionTable->GetRow(i);
                        if (strcmp(row->Get(0), tachieCommand[1]) == 0)
                        {
                            strcpy(tachieCommand[1], row->Get(1));
                            strcat(tachieCommand[1], ",");
                            strcat(tachieCommand[1], row->Get(2));

                        }
                    }
                    IdOperator::CombineStringId<'+', MAX_COLUMNS_CONTENT_LENGTH, NUM_OF_TACHIE_COMMAND>(token->m_content, tachieCommand);

                }
                break;


                default:
                    break;
                }

            }
            else if (token->m_tokeType == StoryJsonContentCompiler::TokenType::TokenOpSlash)
            {
                //Start close
                isReadCloseLabel = true;
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
        m_currentState = CompilerState::StateString;
        m_lastState = CompilerState::StateString;
    }

}
