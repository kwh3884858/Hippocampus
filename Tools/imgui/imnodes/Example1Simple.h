//
//  Example1.h
//  example_osx_opengl2
//
//  Created by 威化饼干 on 26/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef Example1Sample_h
#define Example1Sample_h

#include "ExampleBase.h"
namespace  Example
{
    class Example1 :public ExampleBase
    {
        virtual void NodeEditorInitialize() override;
        virtual void NodeEditorShow()override;
        virtual void NodeEditorShutdown()override;
    }; // namespace example

}

#endif /* Example1Sample_h */
