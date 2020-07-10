#ifndef JsonUtility_h
#define JsonUtility_h

#include "nlohmann/json.hpp"

namespace HeavenGateEditor {
    using json = nlohmann::json;

    void GetContentException(char*const des, const json & j, const char* const index);
    void GetJsonException(json & des, const json& src, const char* const index);
}

#endif /* JsonUtility_h */
