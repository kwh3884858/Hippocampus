//
//  StoryJsonJumpNode.cpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#include "StoryJsonJumpNode.h"

namespace HeavenGateEditor {
enum class JumpLayout
{
    NodeTypeName = 0,
    Jump = 1,
    Content
};

extern char jumpNodeString[][MAX_ENUM_LENGTH] = {
    "typename",
    "jump",
    "content"
};



StoryJump::StoryJump()
{
    m_nodeType = NodeType::Jump;
}

StoryJump::StoryJump(const StoryJump& storyJump)
{
    m_nodeType = storyJump.m_nodeType;
    strcpy(m_jumpId, storyJump.m_jumpId);
    strcpy(m_jumpContent, storyJump.m_jumpContent);

}

void to_json(json& j, const StoryJump& p)
{
    j = json{
        {jumpNodeString[(int)JumpLayout::NodeTypeName],      nodeTypeString[(int)p.m_nodeType] },
        {jumpNodeString[(int)JumpLayout::Jump],          p.m_jumpId},
        {jumpNodeString[(int)JumpLayout::Content],          p.m_jumpContent}

    };
}

void from_json(const json& j, StoryJump& p) {
    p.m_nodeType = NodeType::Jump;
    GetContentException(p.m_jumpId, j, jumpNodeString[(int)JumpLayout::Jump]);
    GetContentException(p.m_jumpContent, j, jumpNodeString[(int)JumpLayout::Content]);
    /*      strcpy(p.m_jumpId, j.at(jumpNodeString[(int)JumpLayout::Jump]).get_ptr<const json::string_t *>()->c_str());
          strcpy(p.m_jumpContent, j.at(jumpNodeString[(int)JumpLayout::Content]).get_ptr<const json::string_t *>()->c_str());*/
}



}
