#pragma once

#ifndef StoryJsonContentCompiler_h
#define StoryJsonContentCompiler_h

#include "StorySingleton.h"
#include "HeavenGateEditorConstant.h"
#include <vector>

namespace HeavenGateEditor {

    using std::vector;

    class StoryJson;
    class StoryJsonContentCompiler final : public StorySingleton<StoryJsonContentCompiler>
    {
    public:
        enum class CompilerState {
            Error,
            StateString,
            StateProperty,
            StateValue,
            StateStartBracket,
            StateStopBracket,
            StateOp,
        };

        enum class TokenType
        {
            TokenIdnet,
            TokenInstructor,
            TokenStringID,

            TokenOpEquel,
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

        void CompilerContent(StoryJson* const storyJson);

    private:
        
        void Lexer(const StoryJson* const storyJson);
        void Parser();

        void SwitchCompilerState(CompilerState state);

        vector<Token> m_tokens;
        CompilerState m_state;
        CompilerState m_lastState;
    };
}

#endif
