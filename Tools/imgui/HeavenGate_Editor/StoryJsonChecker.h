#pragma once


#ifndef StoryJsonChecker_h
#define StoryJsonChecker_h

#include "StorySingleton.h"

namespace HeavenGateEditor {
    class StoryJson;

    class StoryJsonChecker final : public StorySingleton<StoryJsonChecker>
    {
    public:
        StoryJsonChecker() = default;
        virtual ~StoryJsonChecker() override = default;
        //initialize function, take the place of constructor
        virtual bool Initialize() override { return true; };
        //destroy function, take the  place of destructor
        virtual bool Shutdown() override { return true; };

        bool CheckJsonStory(const StoryJson* const story) const;

    private:
        //Checker mainly prevents two behavior:
        //One: A label node`s next node is corresponding jump node,
        //is will cause a jump loop.

        //Two: A label of a sequence of label list, and corresponding jump node in the next node
        //is will case jump loop too.

        //The simple way to check these situations is to make sure at lease one non-corresponding-jump next to the label node.
        bool CheckLabelAndJumpPosition(const StoryJson* const story) const;

    };


}

#endif /* StoryJsonChecker_h */
