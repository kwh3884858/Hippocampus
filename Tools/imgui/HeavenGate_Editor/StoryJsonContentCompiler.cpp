#include "StoryJsonContentCompiler.h"

#include "StoryJson.h"

namespace HeavenGateEditor {


    void StoryJsonContentCompiler::CompilerContent(StoryJson* const storyJson)
    {
        m_state = CompilerState::StateString;
        char tmp[MAX_CONTENT];
        memset(tmp, '\0', sizeof(tmp));

        for (int i = 0; i < storyJson->Size(); i++)
        {
            const StoryNode* const node = storyJson->GetNode(i);

            if (node->m_nodeType != NodeType::Word) continue;

            const StoryWord* const word = static_cast<const StoryWord* const>(node);

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
                    break;
                case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateProperty: {
                    Token* token = new Token;
                    token->m_tokeType = TokenType::TokenInstructor;
                    
                    break;
                }
                case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateValue:
                    break;
                case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStartBracket:
                    break;
                case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateStopBracket:
                    break;
                case HeavenGateEditor::StoryJsonContentCompiler::CompilerState::StateOp:
                    break;
                default:
                    break;
                }
            }

        }
    }

    void StoryJsonContentCompiler::Lexer(const StoryJson* const storyJson)
    {

    }

    void StoryJsonContentCompiler::Parser()
    {

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
            }else if (m_state == CompilerState::StateStartBracket)
            {
                m_state = CompilerState::StateProperty;
            }else if (m_state == CompilerState::StateOp)
            {
                m_state = CompilerState::StateValue;
            }

            break;
        }
        case CompilerState::StateProperty:
        {
            if (m_state == CompilerState::StateStartBracket)
            {
                m_state = state;
            }

            break;

        }
        case CompilerState::StateValue:
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
            if (m_state == CompilerState::StateValue)
            {
                m_state = state;
            }

            break;
        }
        case CompilerState::StateOp: {
            if (m_state == CompilerState::StateProperty)
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

}
