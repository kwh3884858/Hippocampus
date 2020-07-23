

#include "StoryJsonEventNode.h"
#include "JsonUtility.h"

namespace HeavenGateEditor {

    enum class EventLayout {
        NodeTypeName = 0,
        EventType,
        EventName

    };

    extern char eventString[][MAX_ENUM_LENGTH] = {
        "typename",
        "eventType",
        "eventName"
    };

    enum class EventType {
        InvokeEvent,
        LoadMission,
        LoadScene,
        PlayAnimation,
        Amount
    };

    extern char eventTypeString[][MAX_ENUM_LENGTH] = {
        "invokeEvent",
        "loadMission",
        "loadScene",
        "playAnimation"
    };

    extern int EventTypeAmount = (int)EventType::Amount;

    StoryEvent::StoryEvent() {
        m_nodeType = NodeType::raiseEvent;
        m_eventType = EventType::InvokeEvent;
    }

    StoryEvent::StoryEvent(const StoryEvent& storyEvent) {
        m_nodeType = NodeType::raiseEvent;
        strcpy(m_eventName, storyEvent.m_eventName);
        m_eventType = storyEvent.m_eventType;
    }


    void to_json(json& j, const StoryEvent& p) {
        j = json{
              {eventString[(int)EventLayout::NodeTypeName],  nodeTypeString[(int)p.m_nodeType] },
            {eventString[(int)EventLayout::EventType],         eventTypeString[(int)p.m_eventType]},
              {eventString[(int)EventLayout::EventName],          p.m_eventName}
        };
    }
    void from_json(const json& j, StoryEvent& p) {
        p.m_nodeType = NodeType::raiseEvent;
        char tmpEventType[MAX_ENUM_LENGTH];
        GetContentException(tmpEventType, j, eventString[(int)EventLayout::EventType]);
        GetContentException(p.m_eventName, j, eventString[(int)EventLayout::EventName]);

        //Default to invoke event
        p.m_eventType = EventType::InvokeEvent;
        //Different event type
        if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadMission]) == 0) {
            p.m_eventType = EventType::LoadMission;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::InvokeEvent]) == 0) {
            p.m_eventType = EventType::InvokeEvent;
        }else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadScene]) == 0){
            p.m_eventType = EventType::LoadScene;
        }else if (strcmp(tmpEventType, eventTypeString[(int)EventType::PlayAnimation]) == 0){
            p.m_eventType = EventType::PlayAnimation;
        }
    }

}
