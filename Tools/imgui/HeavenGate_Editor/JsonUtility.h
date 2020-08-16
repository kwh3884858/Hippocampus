#ifndef JsonUtility_h
#define JsonUtility_h

#include "nlohmann/json.hpp"

namespace HeavenGateEditor {
    using json = nlohmann::json;

    void GetCharPointerException(char*const des, const json & j, const char* const index);
    void GetJsonException(json & des, const json& src, const char* const index);

    template<typename T>
    void GetContentException(T des, const json& j, const char * const index);


    template<typename T>
    void GetContentException(T des, const json& j, const char * const index)
    {
        try
        {
            des = j.at(index).get<T>();
        }
        catch (json::exception& e)
        {
            printf("\n --!-- Get Content Exception\n message: %s \n exception id: %d \n lack of: %s \n\n", e.what(), e.id, index);
        }
    }
}

#endif /* JsonUtility_h */
