#pragma once
#include "nlohmann/json.hpp"

namespace HeavenGateEditor {
    using json = nlohmann::json;

    enum class UniqueIdLayout :int;
    extern char uniqueIdString[][MAX_ENUM_LENGTH];
    class StoryJsonUniqueId
    {
        friend void to_json(json& j, const StoryJsonUniqueId& p);
        friend void from_json(const json& j, StoryJsonUniqueId& p);
    public:
        StoryJsonUniqueId();
        ~StoryJsonUniqueId();

        bool IsValid();
    private:
        static const int INVALID_ID;

        unsigned long int m_id;
    };

    void to_json(json& j, const StoryJsonUniqueId& p);
    void from_json(const json& j, StoryJsonUniqueId& p);

}
