//
//  StoryJsonJumpNode.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef StoryJsonJumpNode_h
#define StoryJsonJumpNode_h

#include "StoryJsonStoryNode.h"

namespace HeavenGateEditor {

    enum class JumpLayout :int;
    extern char jumpNodeString[][MAX_ENUM_LENGTH];

class StoryJump : public StoryNode
{
public:
    char m_jumpId[MAX_ID];
    char m_jumpContent[MAX_CONTENT];

    StoryJump();
    StoryJump(const StoryJump& storyJump);
};

void to_json(json& j, const StoryJump& p);
void from_json(const json& j, StoryJump& p);

}
#endif /* StoryJsonJumpNode_h */
