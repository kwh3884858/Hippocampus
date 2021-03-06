

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
        LoadCgScene,
        CloseCgScene,
        LoadControversy,
        PlayCutIn,
        PlayInteractionAnimation,
        PlayAnimation,
        PlayTimeline,
        LoadFrontground,
        LoadBackground,
        LoadSkybox,
        SwitchTalkUIType,
        RemoveSpecificExhibit,
        RemoveAllExhibit,
        GameOver,
        Amount
    };

    extern char eventTypeString[][MAX_ENUM_LENGTH] = {
        "invokeEvent",
        "loadMission",
        "loadScene",
        "LoadCgScene",
        "CloseCgScene",
        "LoadControversy",
        "PlayCutIn",
        "PlayInteractionAnimation",
        "playAnimation",
        "PlayTimeline",
        "LoadFrontground",
        "LoadBackground",
        "LoadSkybox",
        "SwitchTalkUIType",
        "RemoveSpecificExhibit",
        "RemoveAllExhibit",
        "GameOver"
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
              {eventString[(int)EventLayout::NodeTypeName],      nodeTypeString[(int)p.m_nodeType] },
              {eventString[(int)EventLayout::EventType],         eventTypeString[(int)p.m_eventType]},
              {eventString[(int)EventLayout::EventName],         p.m_eventName}
        };
    }
    void from_json(const json& j, StoryEvent& p) {
        p.m_nodeType = NodeType::raiseEvent;
        char tmpEventType[MAX_ENUM_LENGTH];
        GetCharPointerException(tmpEventType, j, eventString[(int)EventLayout::EventType]);
        GetCharPointerException(p.m_eventName, j, eventString[(int)EventLayout::EventName]);

        //Default to invoke event
        p.m_eventType = EventType::InvokeEvent;
        //Different event type
        if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadMission]) == 0) {
            p.m_eventType = EventType::LoadMission;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::InvokeEvent]) == 0) {
            p.m_eventType = EventType::InvokeEvent;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadScene]) == 0) {
            p.m_eventType = EventType::LoadScene;
        }
        else if(strcmp(tmpEventType, eventTypeString[(int)EventType::LoadCgScene]) == 0){
            p.m_eventType = EventType::LoadCgScene;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::CloseCgScene]) == 0) {
            p.m_eventType = EventType::CloseCgScene;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadControversy]) == 0){
            p.m_eventType = EventType::LoadControversy;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::PlayCutIn]) == 0) {
            p.m_eventType = EventType::PlayCutIn;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::PlayInteractionAnimation]) == 0){
            p.m_eventType = EventType::PlayInteractionAnimation;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::PlayAnimation]) == 0) {
            p.m_eventType = EventType::PlayAnimation;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::PlayTimeline]) == 0) {
            p.m_eventType = EventType::PlayTimeline;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadFrontground]) == 0){
            p.m_eventType = EventType::LoadFrontground;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadBackground]) == 0) {
            p.m_eventType = EventType::LoadBackground;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::LoadSkybox]) == 0){
            p.m_eventType = EventType::LoadSkybox;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::SwitchTalkUIType]) == 0){
            p.m_eventType = EventType::SwitchTalkUIType;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::RemoveSpecificExhibit]) == 0){
            p.m_eventType = EventType::RemoveSpecificExhibit;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::RemoveAllExhibit]) == 0){
            p.m_eventType = EventType::RemoveAllExhibit;
        }
        else if (strcmp(tmpEventType, eventTypeString[(int)EventType::GameOver]) == 0){
            p.m_eventType = EventType::GameOver;
        }
    }

}
