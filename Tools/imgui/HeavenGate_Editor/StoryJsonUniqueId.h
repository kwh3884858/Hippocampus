#pragma once
#include "nlohmann/json.hpp"
#include "HeavenGateEditorConstant.h"

namespace HeavenGateEditor {
    class StoryJson;
    using json = nlohmann::json;
    enum class UniqueIdLayout :int;
    extern char uniqueIdString[][MAX_ENUM_LENGTH];
    void from_json(const json & j, StoryJson & p);

class StoryJsonUniqueId
    {
        friend void from_json(const json & j, StoryJson & p); // Generate ID for old file format that lack of ID.
        friend void to_json(json& j, const StoryJsonUniqueId& p);
        friend void from_json(const json& j, StoryJsonUniqueId& p);
    public:
        static const UniqueID INVALID_ID;

        StoryJsonUniqueId();
        StoryJsonUniqueId(const StoryJsonUniqueId& uniqueId);
        ~StoryJsonUniqueId();

        bool IsValid();
        UniqueID GetId();
    private:
        //Only for friend
        void GenerateId();

        UniqueID m_id;
    };

    void to_json(json& j, const StoryJsonUniqueId& p);
    void from_json(const json& j, StoryJsonUniqueId& p);

}
