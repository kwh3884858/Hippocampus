#pragma once

#ifndef StoryJsonContentCompiler_h
#define StoryJsonContentCompiler_h

#include "StorySingleton.h"
#include "HeavenGateEditorConstant.h"
#include <vector>

namespace HeavenGateEditor {

    using std::vector;

    class StoryJson;
    class StoryWord;

    class StoryJsonContentCompiler final : public StorySingleton<StoryJsonContentCompiler>
    {
    public:
        enum class CompilerState {
            Error,
            StateString,
            StateInstructor,
            StateIdent,
            StateStartBracket,
            StateStopBracket,
            StateOp,
        };

        enum class TokenType
        {
            TokenNone,

            TokenIdnet,
            TokenInstructor,
            TokenCloseLabel,

            TokenContent,

            TokenOpBracketLeft,
            TokenOpBracketRight,
            TokenOpColon

        };

 
        struct Token
        {
            TokenType m_tokeType;
            char m_content[MAX_CONTENT];

            Token() {
                m_tokeType = TokenType::TokenNone;
            }

            Token(const Token& token) {
                m_tokeType = token.m_tokeType;
                strcpy(m_content, token.m_content);
            }
        };

        StoryJsonContentCompiler() = default;
        virtual ~StoryJsonContentCompiler() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override { return true; };
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override { Clear();  return true; };

        void Compile(StoryJson* const storyJson);
        vector<Token*> CompileToTokens(StoryWord* const storyWord);
    private:
        void CompileEach(StoryWord* const word);
        void Lexer(const StoryWord* const storyJson);
        void Parser();
        void Output(StoryWord* const word);
        void Clear();

        void SwitchCompilerState(CompilerState lastState);
        void CreateToken(char*const aString, CompilerState tokenType);
        Token*const CreateTokenByString(char*const aString, TokenType tokenType);

        void AddToken(Token*const token );

        vector<Token*> m_tokens;
        CompilerState m_state;
        CompilerState m_lastState;
    };
}

#endif
