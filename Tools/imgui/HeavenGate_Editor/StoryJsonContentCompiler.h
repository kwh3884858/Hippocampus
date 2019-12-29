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
        };

        StoryJsonContentCompiler() = default;
        virtual ~StoryJsonContentCompiler() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override { return true; };
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override { return true; };

        void Compile(StoryJson* const storyJson);

    private:
        void CompileEach(const StoryWord* const word);


        void Lexer(const StoryWord* const storyJson);

        //void Lexer(const StoryJson* const storyJson);
        void Parser();

        void SwitchCompilerState(CompilerState state);
        Token*const CreateTokenByString(char*const aString, TokenType tokenType);

        void AddToken(Token*const token );

        vector<Token*> m_tokens;
        CompilerState m_state;
        CompilerState m_lastState;
    };
}

#endif
