//
//  StoryJsonLabelNode.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef StoryJsonLabelNode_h
#define StoryJsonLabelNode_h

#include "StoryJsonStoryNode.h"

namespace HeavenGateEditor {

enum class LabelLayout :int;
extern char labelNodeString[][MAX_ENUM_LENGTH];

class StoryLabel :public StoryNode
{

public:
    char m_labelId[MAX_ID];

    StoryLabel();
    StoryLabel(const StoryLabel& storyLabel);




};

void to_json(json& j, const StoryLabel& p);
void from_json(const json& j, StoryLabel& p);
}
#endif /* StoryJsonLabelNode_h */
