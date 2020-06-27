#ifndef StoryJsonEndNode_h
#define StoryJsonEndNode_h

#include "StoryJsonStoryNode.h"
namespace HeavenGateEditor {

    enum class EndLayout :int;
    extern char endNodeString[][MAX_ENUM_LENGTH];

    class StoryEnd : public StoryNode
    {
    public:
        StoryEnd();
        StoryEnd(const StoryEnd& storyEnd);
    };

    void to_json(json& j, const StoryEnd& p);
    void from_json(const json& j, StoryEnd& p);
}


#endif /* StoryJsonEndNode_h */
