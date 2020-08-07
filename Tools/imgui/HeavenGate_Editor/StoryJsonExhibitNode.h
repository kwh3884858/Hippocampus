//
//  StoryJsonExhibitNode.hpp
//  example_osx_opengl2
//
//  Created by 威化饼干 on 22/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef StoryJsonExhibitNode_h
#define StoryJsonExhibitNode_h

#include "StoryJsonStoryNode.h"

namespace HeavenGateEditor {

enum class ExhibitLayout : int;
extern char exhibitString[][MAX_ENUM_LENGTH];

class StoryExhibit: public StoryNode{


public:
    char m_exhibitID[MAX_EXHIBIT_NAME];

    StoryExhibit();
    StoryExhibit(const StoryExhibit& storyExhibit);
};

void to_json(json& j, const StoryExhibit& p);
void from_json(const json& j, StoryExhibit& p);


}
#endif /* StoryJsonExhibitNode_h */
