#include "StoryJsonUniqueId.h"
#include <sys/time.h>
#include <sys/utime.h>

namespace HeavenGateEditor {

    const int StoryJsonUniqueId::INVALID_ID = 0;

    enum class UniqueIdLayout {
        ID
    };
    extern char uniqueIdString[][MAX_ENUM_LENGTH] = {
        "id",
    };

    StoryJsonUniqueId::StoryJsonUniqueId()
    {
        m_id = INVALID_ID;
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
        m_id = std::time(nullptr);
    }

    void to_json(json& j, const StoryJsonUniqueId& p)
    {
        j = json{
          {uniqueIdString[(int)UniqueIdLayout::ID],    p.m_id },
        };
    }

    void from_json(const json& j, StoryJsonUniqueId& p)
    {
        GetCharPointerException(p.m_id, j, uniqueIdString[(int)UniqueIdLayout::ID]);
    }

}
