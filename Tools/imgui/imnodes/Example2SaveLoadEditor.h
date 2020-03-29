//
//  Example2SaveLoadEditor.h
//  example_osx_opengl2
//
//  Created by 威化饼干 on 26/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef Example2SaveLoadEditor_h
#define Example2SaveLoadEditor_h
#include "ExampleBase.h"
namespace  Example
{
    class Example2 :public ExampleBase
    {
        virtual void NodeEditorInitialize() override;
        virtual void NodeEditorShow()override;
        virtual void NodeEditorShutdown()override;
    }; // namespace example

}
#endif /* Example2SaveLoadEditor_h */
