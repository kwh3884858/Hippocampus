//
//  StoryJsonWordNode.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef StoryJsonWordNode_h
#define StoryJsonWordNode_h

#include "StoryJsonStoryNode.h"

namespace HeavenGateEditor {

    enum class WordLayout :int;

    extern char wordNodeString[][MAX_ENUM_LENGTH];



    class StoryWord :public StoryNode {
    public:

        char m_name[MAX_NAME];
        char m_content[MAX_CONTENT];

        StoryWord();
        StoryWord(const StoryWord& storyWard);
    };

    void to_json(json& j, const StoryWord& p);
    void from_json(const json& j, StoryWord& p);

}


#endif /* StoryJsonWordNode_h */
