// GENERATED AUTOMATICALLY FROM 'Assets/code/Input/InputActions_Player.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions_Player : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions_Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions_Player"",
    ""maps"": [
        {
            ""name"": ""Player Controls"",
            ""id"": ""cb4067f3-a685-4c86-b9ea-46a6eabfada7"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""548e32fd-77d1-40e5-8197-32ca56b41bc0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""8ebbde1f-3044-41bc-bdac-430e0eae1a68"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""10fecf58-9483-40e0-ae20-15c8d01a4288"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""6f383b9f-7338-4e17-9f1e-ed70232a6bd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""a70a208e-b491-4921-b460-a0144030ef2a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""12fd19a0-3386-42fc-bd3d-0a46feabcc0c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Map"",
                    ""type"": ""Button"",
                    ""id"": ""0174e945-0e92-4956-b69f-4953c4392dc1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ToggleConsole"",
                    ""type"": ""Button"",
                    ""id"": ""1eb8365b-19e8-43b4-a6fc-529b2f698ddc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""550559c3-1cd9-452a-9bd1-d556f1d051dc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""99258992-afbc-4513-a4ee-24146566e341"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d57e0987-ea9f-4b18-9042-239931d4c060"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""75168890-922f-4122-9968-1ecac0f33c28"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""54b99838-0c45-421e-af38-b1f25b3f6927"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77680fb0-0b9d-4a74-97de-9e3149ad6526"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""df04a4e1-fc36-4ebd-b050-536736220da7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42716e15-291c-4c71-8ded-0d03279959df"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bc14fbd-1107-4761-aa6f-b3367d7705e6"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e6d6425-a990-4434-b58d-57464db363d4"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a438aa20-6e2c-4d33-86db-ba52748443e6"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""341750bc-2769-4dd1-98ab-337f62d7143c"",
                    ""path"": ""<SwitchProControllerHID>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4958579c-56d7-444a-9024-680f58440e32"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43d1488b-4ff2-4456-b1bf-a368a70fb680"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d23f9eb5-e32b-417b-8edf-d10cd6bdc1d3"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d414f035-074d-443f-9967-05cbc5b2694b"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b3cd3f7-7c22-49d9-96eb-7e35b1caf2f4"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a181af6e-48b2-41b4-9f09-61bbc2a945b2"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""265a85a7-44ef-4eb3-a34a-3e4d61caf3b2"",
                    ""path"": ""<DualShock4GampadiOS>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eee70c4a-5c4d-4e55-b8e2-abfeafb2ea4b"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59369985-1c5c-46ee-af8b-492fbc25c3ab"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70467786-dbfd-45ec-93f3-cda13725378f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1d3e88aa-dbdd-4434-92e2-7e1e42e10d9b"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""763b2814-1d83-46fe-825e-6ee650b3a761"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad9f3a85-dfae-4554-80d0-c6b014605ee6"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Map"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc409e34-43a4-4e0a-9fa1-15fbe6584df5"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleConsole"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35b5f2cc-0395-4665-9e9e-47353c6c7075"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfa1bdd4-2248-480e-8e1c-e8f2faf25a5f"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu Controls"",
            ""id"": ""0914fb5b-51f6-4b26-9ed7-a3e72d065118"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""538ffe95-ba92-4acb-84f7-314f6ac8e0a5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Left Click"",
                    ""type"": ""PassThrough"",
                    ""id"": ""96c8be88-a7bb-4861-b5e9-956b4208d043"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""d54e5ff5-4f35-4d2f-a745-95d14aef8c43"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Confirm"",
                    ""type"": ""Button"",
                    ""id"": ""e886c953-4bee-458d-850f-b0ba48a9b5f4"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""28ea354d-f474-4b35-8f3b-60c271f0c506"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""TogglePause"",
                    ""type"": ""Button"",
                    ""id"": ""15d7825a-28d6-4a09-9500-a3da452ffd92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad Right Stick"",
                    ""id"": ""c1491510-9d0f-47b0-868e-99575e46d097"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""57fb7217-68c1-483e-a15b-0fd1e5ab3fc3"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0c09243f-be8c-44a1-87c4-a0d3ca3a27a5"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD Keys"",
                    ""id"": ""c2a77ff0-1ce1-4c49-a4dd-94601087a2a2"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a67b96dc-9151-496b-9be2-b4d65a82f52a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4f8ac3a8-5653-4cf1-9687-259b7e6bfca4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a3da140b-c504-4aea-9824-ffd10d44e52a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d2cdc452-d127-4c2c-b57c-1f78e29cb425"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a16b4641-1591-4d94-9fd4-e1eafd539931"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""341f36e1-889b-4d62-834f-622378da658d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Gamepad Left Stick"",
                    ""id"": ""c2c92ef2-a9d0-4393-86c7-4180acc16b6d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bed70561-f1cc-4c56-9715-66475aa6437f"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0005d032-151a-4ee0-8127-110d55e5ed9d"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""84fcadd5-5853-4142-b3f9-58a5ab2ad788"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""233f731d-8d73-4761-8879-66c0e0da124d"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d687f18d-7559-488c-8542-e3da3a3dd1f7"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Arrow Keys"",
                    ""id"": ""c50ac654-ca86-486c-b427-057a0aacbb3b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""80a43030-09a8-4324-b825-39a685b9a975"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0284caff-9cfb-477f-901c-c6be4082785f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""359d5348-82b3-4e60-9536-8c817495d31a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b77717c3-b7e9-450f-8bc2-3aa284fac5cd"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b5076a57-fe62-4632-8d6c-da0844960a14"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7658a78-a141-4f0c-beb5-0a6a3e393c7b"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14559c94-e8a7-426b-8687-fa5f1420a0c1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b35f103a-716c-4078-ad8d-66c5fb7fbb45"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31d35f5d-9c6e-48a0-b35b-183ae60ce844"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ac30c5a-1374-4b87-a4f1-de4829d372dc"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcc91d04-d89b-403f-8ea9-9b694530ccce"",
                    ""path"": ""<SwitchProControllerHID>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Confirm"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5baf6b69-6d85-480f-87dd-6d85a2462b99"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""749af94a-e53c-4f36-a8ec-0459fd42f676"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10d0dc22-5af5-4204-b6e5-60972a96c116"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""782a65a0-d124-4915-88f2-0e8697096daf"",
                    ""path"": ""<DualShock4GampadiOS>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b50b17fc-1d60-4522-b39d-c67843439854"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touchscreen"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16fbd8f3-c669-4bdb-b737-67cb9d30c065"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d445776d-dfe5-4385-8723-fbff7b5b4ade"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3edc710b-1167-4b85-b1ac-ed4aba4e404a"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TogglePause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Console"",
            ""id"": ""edce0c86-354c-40c9-ac9a-aaa12a1969a4"",
            ""actions"": [
                {
                    ""name"": ""PageUp"",
                    ""type"": ""Button"",
                    ""id"": ""8dfa6ba4-3ff6-486f-a4d6-1aa3d0d276b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PageDown"",
                    ""type"": ""Button"",
                    ""id"": ""83a0e778-e4be-43ee-8227-db353780fdc1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""453753d1-489b-46b7-b231-dd187eeff5c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkipTimeline"",
                    ""type"": ""Button"",
                    ""id"": ""ea91e80b-3b95-4534-bc30-ddb73dad1af8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b5869f61-7df1-47d5-9994-42f78373dad1"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57fc2aa8-67a2-4f94-84a5-6ee0acc6ef0d"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PageDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d6e7c57f-71ea-4cd9-9dd9-b896af27064a"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60c07421-e78c-480b-927e-85d15ddc35ab"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b9420ad-5bbf-474c-9475-6d2ea7d04c6e"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipTimeline"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Controversy"",
            ""id"": ""e444cc46-3177-4234-93cb-8c45421456d4"",
            ""actions"": [
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""014bb851-27f4-4946-a7c6-fd9a5a7ffd4e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""57f7d291-69e0-4526-bebf-a38e897dd6b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a9a6e80a-370d-48ed-9cf4-7a4be9de0c58"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7243d13-8591-464a-955b-ec7f909a7871"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a04e397b-c2a7-43fb-9652-ea121a0fbe4e"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4d69b49-5e0a-4e4a-98fb-8e1a0ec29ac9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4b9b66d-65d0-4f79-85c2-14fdc23becb9"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""312e7296-caa1-4f00-b089-87129e977834"",
                    ""path"": ""<DualShockGamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90f1d8dc-d223-4f40-a9fe-575181a2572d"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21c13efb-314e-40a3-a87f-bf6fd0aaa2fe"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""StoryPlayer"",
            ""id"": ""551fcefc-58c6-429c-8725-bd3a6aee3efb"",
            ""actions"": [
                {
                    ""name"": ""Next"",
                    ""type"": ""Button"",
                    ""id"": ""5bf056ec-904c-4686-878a-68af9b38ff2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""119ef671-6f10-42fa-87ec-676674890ad5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Next"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard And Mouse"",
            ""bindingGroup"": ""Keyboard And Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touchscreen"",
            ""bindingGroup"": ""Touchscreen"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player Controls
        m_PlayerControls = asset.FindActionMap("Player Controls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Look = m_PlayerControls.FindAction("Look", throwIfNotFound: true);
        m_PlayerControls_Confirm = m_PlayerControls.FindAction("Confirm", throwIfNotFound: true);
        m_PlayerControls_Cancel = m_PlayerControls.FindAction("Cancel", throwIfNotFound: true);
        m_PlayerControls_TogglePause = m_PlayerControls.FindAction("TogglePause", throwIfNotFound: true);
        m_PlayerControls_Interact = m_PlayerControls.FindAction("Interact", throwIfNotFound: true);
        m_PlayerControls_Map = m_PlayerControls.FindAction("Map", throwIfNotFound: true);
        m_PlayerControls_ToggleConsole = m_PlayerControls.FindAction("ToggleConsole", throwIfNotFound: true);
        m_PlayerControls_Sprint = m_PlayerControls.FindAction("Sprint", throwIfNotFound: true);
        // Menu Controls
        m_MenuControls = asset.FindActionMap("Menu Controls", throwIfNotFound: true);
        m_MenuControls_Navigate = m_MenuControls.FindAction("Navigate", throwIfNotFound: true);
        m_MenuControls_LeftClick = m_MenuControls.FindAction("Left Click", throwIfNotFound: true);
        m_MenuControls_Point = m_MenuControls.FindAction("Point", throwIfNotFound: true);
        m_MenuControls_Confirm = m_MenuControls.FindAction("Confirm", throwIfNotFound: true);
        m_MenuControls_Cancel = m_MenuControls.FindAction("Cancel", throwIfNotFound: true);
        m_MenuControls_TogglePause = m_MenuControls.FindAction("TogglePause", throwIfNotFound: true);
        // Console
        m_Console = asset.FindActionMap("Console", throwIfNotFound: true);
        m_Console_PageUp = m_Console.FindAction("PageUp", throwIfNotFound: true);
        m_Console_PageDown = m_Console.FindAction("PageDown", throwIfNotFound: true);
        m_Console_Enter = m_Console.FindAction("Enter", throwIfNotFound: true);
        m_Console_SkipTimeline = m_Console.FindAction("SkipTimeline", throwIfNotFound: true);
        // Controversy
        m_Controversy = asset.FindActionMap("Controversy", throwIfNotFound: true);
        m_Controversy_LightAttack = m_Controversy.FindAction("LightAttack", throwIfNotFound: true);
        m_Controversy_HeavyAttack = m_Controversy.FindAction("HeavyAttack", throwIfNotFound: true);
        // StoryPlayer
        m_StoryPlayer = asset.FindActionMap("StoryPlayer", throwIfNotFound: true);
        m_StoryPlayer_Next = m_StoryPlayer.FindAction("Next", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player Controls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Look;
    private readonly InputAction m_PlayerControls_Confirm;
    private readonly InputAction m_PlayerControls_Cancel;
    private readonly InputAction m_PlayerControls_TogglePause;
    private readonly InputAction m_PlayerControls_Interact;
    private readonly InputAction m_PlayerControls_Map;
    private readonly InputAction m_PlayerControls_ToggleConsole;
    private readonly InputAction m_PlayerControls_Sprint;
    public struct PlayerControlsActions
    {
        private @InputActions_Player m_Wrapper;
        public PlayerControlsActions(@InputActions_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Look => m_Wrapper.m_PlayerControls_Look;
        public InputAction @Confirm => m_Wrapper.m_PlayerControls_Confirm;
        public InputAction @Cancel => m_Wrapper.m_PlayerControls_Cancel;
        public InputAction @TogglePause => m_Wrapper.m_PlayerControls_TogglePause;
        public InputAction @Interact => m_Wrapper.m_PlayerControls_Interact;
        public InputAction @Map => m_Wrapper.m_PlayerControls_Map;
        public InputAction @ToggleConsole => m_Wrapper.m_PlayerControls_ToggleConsole;
        public InputAction @Sprint => m_Wrapper.m_PlayerControls_Sprint;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Look.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnLook;
                @Confirm.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnConfirm;
                @Cancel.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCancel;
                @TogglePause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTogglePause;
                @TogglePause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTogglePause;
                @TogglePause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnTogglePause;
                @Interact.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                @Map.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMap;
                @Map.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMap;
                @Map.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMap;
                @ToggleConsole.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleConsole;
                @ToggleConsole.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleConsole;
                @ToggleConsole.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnToggleConsole;
                @Sprint.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @TogglePause.started += instance.OnTogglePause;
                @TogglePause.performed += instance.OnTogglePause;
                @TogglePause.canceled += instance.OnTogglePause;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Map.started += instance.OnMap;
                @Map.performed += instance.OnMap;
                @Map.canceled += instance.OnMap;
                @ToggleConsole.started += instance.OnToggleConsole;
                @ToggleConsole.performed += instance.OnToggleConsole;
                @ToggleConsole.canceled += instance.OnToggleConsole;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);

    // Menu Controls
    private readonly InputActionMap m_MenuControls;
    private IMenuControlsActions m_MenuControlsActionsCallbackInterface;
    private readonly InputAction m_MenuControls_Navigate;
    private readonly InputAction m_MenuControls_LeftClick;
    private readonly InputAction m_MenuControls_Point;
    private readonly InputAction m_MenuControls_Confirm;
    private readonly InputAction m_MenuControls_Cancel;
    private readonly InputAction m_MenuControls_TogglePause;
    public struct MenuControlsActions
    {
        private @InputActions_Player m_Wrapper;
        public MenuControlsActions(@InputActions_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_MenuControls_Navigate;
        public InputAction @LeftClick => m_Wrapper.m_MenuControls_LeftClick;
        public InputAction @Point => m_Wrapper.m_MenuControls_Point;
        public InputAction @Confirm => m_Wrapper.m_MenuControls_Confirm;
        public InputAction @Cancel => m_Wrapper.m_MenuControls_Cancel;
        public InputAction @TogglePause => m_Wrapper.m_MenuControls_TogglePause;
        public InputActionMap Get() { return m_Wrapper.m_MenuControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuControlsActions set) { return set.Get(); }
        public void SetCallbacks(IMenuControlsActions instance)
        {
            if (m_Wrapper.m_MenuControlsActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnNavigate;
                @LeftClick.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnLeftClick;
                @Point.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnPoint;
                @Confirm.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnConfirm;
                @Confirm.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnConfirm;
                @Confirm.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnConfirm;
                @Cancel.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnCancel;
                @TogglePause.started -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
                @TogglePause.performed -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
                @TogglePause.canceled -= m_Wrapper.m_MenuControlsActionsCallbackInterface.OnTogglePause;
            }
            m_Wrapper.m_MenuControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @Confirm.started += instance.OnConfirm;
                @Confirm.performed += instance.OnConfirm;
                @Confirm.canceled += instance.OnConfirm;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @TogglePause.started += instance.OnTogglePause;
                @TogglePause.performed += instance.OnTogglePause;
                @TogglePause.canceled += instance.OnTogglePause;
            }
        }
    }
    public MenuControlsActions @MenuControls => new MenuControlsActions(this);

    // Console
    private readonly InputActionMap m_Console;
    private IConsoleActions m_ConsoleActionsCallbackInterface;
    private readonly InputAction m_Console_PageUp;
    private readonly InputAction m_Console_PageDown;
    private readonly InputAction m_Console_Enter;
    private readonly InputAction m_Console_SkipTimeline;
    public struct ConsoleActions
    {
        private @InputActions_Player m_Wrapper;
        public ConsoleActions(@InputActions_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @PageUp => m_Wrapper.m_Console_PageUp;
        public InputAction @PageDown => m_Wrapper.m_Console_PageDown;
        public InputAction @Enter => m_Wrapper.m_Console_Enter;
        public InputAction @SkipTimeline => m_Wrapper.m_Console_SkipTimeline;
        public InputActionMap Get() { return m_Wrapper.m_Console; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ConsoleActions set) { return set.Get(); }
        public void SetCallbacks(IConsoleActions instance)
        {
            if (m_Wrapper.m_ConsoleActionsCallbackInterface != null)
            {
                @PageUp.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageUp;
                @PageUp.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageUp;
                @PageUp.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageUp;
                @PageDown.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageDown;
                @PageDown.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageDown;
                @PageDown.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnPageDown;
                @Enter.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnEnter;
                @SkipTimeline.started -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSkipTimeline;
                @SkipTimeline.performed -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSkipTimeline;
                @SkipTimeline.canceled -= m_Wrapper.m_ConsoleActionsCallbackInterface.OnSkipTimeline;
            }
            m_Wrapper.m_ConsoleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PageUp.started += instance.OnPageUp;
                @PageUp.performed += instance.OnPageUp;
                @PageUp.canceled += instance.OnPageUp;
                @PageDown.started += instance.OnPageDown;
                @PageDown.performed += instance.OnPageDown;
                @PageDown.canceled += instance.OnPageDown;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @SkipTimeline.started += instance.OnSkipTimeline;
                @SkipTimeline.performed += instance.OnSkipTimeline;
                @SkipTimeline.canceled += instance.OnSkipTimeline;
            }
        }
    }
    public ConsoleActions @Console => new ConsoleActions(this);

    // Controversy
    private readonly InputActionMap m_Controversy;
    private IControversyActions m_ControversyActionsCallbackInterface;
    private readonly InputAction m_Controversy_LightAttack;
    private readonly InputAction m_Controversy_HeavyAttack;
    public struct ControversyActions
    {
        private @InputActions_Player m_Wrapper;
        public ControversyActions(@InputActions_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @LightAttack => m_Wrapper.m_Controversy_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_Controversy_HeavyAttack;
        public InputActionMap Get() { return m_Wrapper.m_Controversy; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControversyActions set) { return set.Get(); }
        public void SetCallbacks(IControversyActions instance)
        {
            if (m_Wrapper.m_ControversyActionsCallbackInterface != null)
            {
                @LightAttack.started -= m_Wrapper.m_ControversyActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_ControversyActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_ControversyActionsCallbackInterface.OnLightAttack;
                @HeavyAttack.started -= m_Wrapper.m_ControversyActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.performed -= m_Wrapper.m_ControversyActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.canceled -= m_Wrapper.m_ControversyActionsCallbackInterface.OnHeavyAttack;
            }
            m_Wrapper.m_ControversyActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @HeavyAttack.started += instance.OnHeavyAttack;
                @HeavyAttack.performed += instance.OnHeavyAttack;
                @HeavyAttack.canceled += instance.OnHeavyAttack;
            }
        }
    }
    public ControversyActions @Controversy => new ControversyActions(this);

    // StoryPlayer
    private readonly InputActionMap m_StoryPlayer;
    private IStoryPlayerActions m_StoryPlayerActionsCallbackInterface;
    private readonly InputAction m_StoryPlayer_Next;
    public struct StoryPlayerActions
    {
        private @InputActions_Player m_Wrapper;
        public StoryPlayerActions(@InputActions_Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Next => m_Wrapper.m_StoryPlayer_Next;
        public InputActionMap Get() { return m_Wrapper.m_StoryPlayer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StoryPlayerActions set) { return set.Get(); }
        public void SetCallbacks(IStoryPlayerActions instance)
        {
            if (m_Wrapper.m_StoryPlayerActionsCallbackInterface != null)
            {
                @Next.started -= m_Wrapper.m_StoryPlayerActionsCallbackInterface.OnNext;
                @Next.performed -= m_Wrapper.m_StoryPlayerActionsCallbackInterface.OnNext;
                @Next.canceled -= m_Wrapper.m_StoryPlayerActionsCallbackInterface.OnNext;
            }
            m_Wrapper.m_StoryPlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Next.started += instance.OnNext;
                @Next.performed += instance.OnNext;
                @Next.canceled += instance.OnNext;
            }
        }
    }
    public StoryPlayerActions @StoryPlayer => new StoryPlayerActions(this);
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard And Mouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_TouchscreenSchemeIndex = -1;
    public InputControlScheme TouchscreenScheme
    {
        get
        {
            if (m_TouchscreenSchemeIndex == -1) m_TouchscreenSchemeIndex = asset.FindControlSchemeIndex("Touchscreen");
            return asset.controlSchemes[m_TouchscreenSchemeIndex];
        }
    }
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnConfirm(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnTogglePause(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnMap(InputAction.CallbackContext context);
        void OnToggleConsole(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
    }
    public interface IMenuControlsActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnPoint(InputAction.CallbackContext context);
        void OnConfirm(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnTogglePause(InputAction.CallbackContext context);
    }
    public interface IConsoleActions
    {
        void OnPageUp(InputAction.CallbackContext context);
        void OnPageDown(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnSkipTimeline(InputAction.CallbackContext context);
    }
    public interface IControversyActions
    {
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
    }
    public interface IStoryPlayerActions
    {
        void OnNext(InputAction.CallbackContext context);
    }
}
