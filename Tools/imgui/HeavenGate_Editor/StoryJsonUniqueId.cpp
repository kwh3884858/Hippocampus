#include "StoryJsonUniqueId.h"
#include "JsonUtility.h"

#ifdef _WIN32
#include <time.h>
#else
#include <sys/time.h>
#endif

namespace HeavenGateEditor {

    const UniqueID StoryJsonUniqueId::INVALID_ID = 0;

    enum class UniqueIdLayout {
        ID
    };
    extern char uniqueIdString[][MAX_ENUM_LENGTH] = {
        "id",
    };

    //We holp every id have a valid default value
    StoryJsonUniqueId::StoryJsonUniqueId()
    {
        m_id = INVALID_ID;
        GenerateId();
    }

    StoryJsonUniqueId::StoryJsonUniqueId(const StoryJsonUniqueId& uniqueId)
    {
        m_id = uniqueId.m_id;
    }

    StoryJsonUniqueId::~StoryJsonUniqueId()
    {
    }

    bool StoryJsonUniqueId::IsValid()
    {
        return m_id != INVALID_ID;
    }

    void StoryJsonUniqueId::GenerateId()
    {
        //TODO: Make sure id is valid and no repeat.
        m_id = time(nullptr);
    }

    UniqueID StoryJsonUniqueId::GetId(){
        assert(IsValid() == true);
        return m_id;
    }

    void to_json(json& j, const StoryJsonUniqueId& p)
    {
        j = json{
          {uniqueIdString[(int)UniqueIdLayout::ID],    p.m_id },
        };
    }

    void from_json(const json& j, StoryJsonUniqueId& p)
    {
        GetContentException<UniqueID>(p.m_id, j, uniqueIdString[(int)UniqueIdLayout::ID]);
    }

}
