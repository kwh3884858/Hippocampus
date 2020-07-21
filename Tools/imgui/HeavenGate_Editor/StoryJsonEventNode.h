
//

#ifndef StoryJsonEventNode_h
#define StoryJsonEventNode_h

#include "StoryJsonStoryNode.h"

namespace HeavenGateEditor {

enum class EventLayout : int;
extern char eventString[][MAX_ENUM_LENGTH];

enum class EventType:int;
extern char eventTypeString[][MAX_ENUM_LENGTH];

extern int EventTypeAmount;

class StoryEvent: public StoryNode{


public:
    char m_eventName[MAX_EVENT_NAME];
    EventType m_eventType;

    StoryEvent();
    StoryEvent(const StoryEvent& storyEvent);
};

void to_json(json& j, const StoryEvent& p);
void from_json(const json& j, StoryEvent& p);


}
#endif /* StoryJsonEventNode_h */
