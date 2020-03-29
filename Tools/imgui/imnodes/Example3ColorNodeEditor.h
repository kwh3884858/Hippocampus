//
//  Example3ColorNodeEditor.h
//  example_osx_opengl2
//
//  Created by 威化饼干 on 26/3/2020.
//  Copyright © 2020 ImGui. All rights reserved.
//

#ifndef Example3ColorNodeEditor_h
#define Example3ColorNodeEditor_h
#include "ExampleBase.h"

namespace Example
{
    class Example3 :public ExampleBase
    {
        virtual void NodeEditorInitialize() override;
        virtual void NodeEditorShow()override;
        virtual void NodeEditorShutdown()override;
    };

} // namespace example

#endif /* Example3ColorNodeEditor_h */
