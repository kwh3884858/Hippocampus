using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Skylight
{


	public class ScriptVirtualMachine
	{

		// ---- General ---------------------------------------------------------------------------
		//private const Int32 true = 1;        // true

		//private const Int32 false = 0;      // false



		// ---- Script Loading Error Codes --------------------------------------------------------

		public const Int32 XS_LOAD_OK = 0;                       // Load successful
		public const Int32 XS_LOAD_ERROR_FILE_IO = 1;            // File I/O error (most likely a file
																 // not found error
		public const Int32 XS_LOAD_ERROR_INVALID_XSE = 2;        // Invalid .XSE structure
		public const Int32 XS_LOAD_ERROR_UNSUPPORTED_VERS = 3;   // The format version is unsupported
		public const Int32 XS_LOAD_ERROR_OUT_OF_MEMORY = 4;      // Out of memory
		public const Int32 XS_LOAD_ERROR_OUT_OF_THREADS = 5;     // Out of threads

		// ---- Threading -------------------------------------------------------------------------

		public const Int32 XS_THREAD_PRIORITY_USER = 0;          // User-defined priority
		public const Int32 XS_THREAD_PRIORITY_LOW = 1;           // Low priority
		public const Int32 XS_THREAD_PRIORITY_MED = 2;           // Medium priority
		public const Int32 XS_THREAD_PRIORITY_HIGH = 3;          // High priority

		public const Int32 XS_INFINITE_TIMESLICE = -1;           // Allows a thread to run indefinitely

		// ---- The Host API ----------------------------------------------------------------------

		public const Int32 XS_GLOBAL_FUNC = -1;                  // Flags a host API function as being
																 // global

		// ---- Data Structures -----------------------------------------------------------------------
		public delegate void HostAPIFuncPntr (Int32 iThreadIndex);       // Host API function poInt32er
																		 // alias

		//typedef void ( * HostAPIFuncPntr ) (Int32 iThreadIndex );  



		// ---- Macros --------------------------------------------------------------------------------

		// These macros are used to wrap the XS_Return*FromHost () functions to allow the call to
		// also exit the current function.

		public void XS_Return (Int32 iThreadIndex, Int32 iParamCount)
		{
			XS_ReturnFromHost (iThreadIndex, iParamCount);
			return;
		}

		public void XS_ReturnInt (Int32 iThreadIndex, Int32 iParamCount, Int32 iInt32)
		{
			XS_ReturnIntFromHost (iThreadIndex, iParamCount, iInt32);
			return;
		}

		public void XS_ReturnFloat (Int32 iThreadIndex, Int32 iParamCount, float fFloat)
		{
			XS_ReturnFloatFromHost (iThreadIndex, iParamCount, fFloat);
			return;
		}

		public void XS_ReturnString (Int32 iThreadIndex, Int32 iParamCount, string pstrString)
		{
			XS_ReturnStringFromHost (iThreadIndex, iParamCount, pstrString);
			return;
		}


		private const string EXEC_FILE_EXT = ".XSE"; // Executable file extension

		private const string XSE_ID_STRING = "XSE0";      // Used to validate an .XSE executable

		private const Int32 MAX_THREAD_COUNT = 8;    // The maximum number of scripts that
													 // can be loaded at once. Change this
													 // to support more or less.

		// ---- Operand/Value Types ---------------------------------------------------------------

		private const Int32 OP_TYPE_NULL = -1;          // Uninitialized/null data
		private const Int32 OP_TYPE_INT = 0;          // Int32eger literal value
		private const Int32 OP_TYPE_FLOAT = 1;          // Floating-poInt32 literal value
		private const Int32 OP_TYPE_STRING = 2;      // String literal value
		private const Int32 OP_TYPE_ABS_STACK_INDEX = 3;          // Absolute array index
		private const Int32 OP_TYPE_REL_STACK_INDEX = 4;          // Relative array index
		private const Int32 OP_TYPE_INSTR_INDEX = 5;          // Instruction index
		private const Int32 OP_TYPE_FUNC_INDEX = 6;          // Function index
		private const Int32 OP_TYPE_HOST_API_CALL_INDEX = 7;          // Host API call index
		private const Int32 OP_TYPE_REG = 8;          // Register

		private const Int32 OP_TYPE_STACK_BASE_MARKER = 9;          // Marks a stack base

		// ---- Instruction Opcodes ---------------------------------------------------------------

		private const Int32 INSTR_MOV = 0;

		private const Int32 INSTR_ADD = 1;
		private const Int32 INSTR_SUB = 2;
		private const Int32 INSTR_MUL = 3;
		private const Int32 INSTR_DIV = 4;
		private const Int32 INSTR_MOD = 5;
		private const Int32 INSTR_EXP = 6;
		private const Int32 INSTR_NEG = 7;
		private const Int32 INSTR_INC = 8;
		private const Int32 INSTR_DEC = 9;

		private const Int32 INSTR_AND = 10;
		private const Int32 INSTR_OR = 11;
		private const Int32 INSTR_XOR = 12;
		private const Int32 INSTR_NOT = 13;
		private const Int32 INSTR_SHL = 14;
		private const Int32 INSTR_SHR = 15;

		private const Int32 INSTR_CONCAT = 16;
		private const Int32 INSTR_GETCHAR = 17;
		private const Int32 INSTR_SETCHAR = 18;

		private const Int32 INSTR_JMP = 19;
		private const Int32 INSTR_JE = 20;
		private const Int32 INSTR_JNE = 21;
		private const Int32 INSTR_JG = 22;
		private const Int32 INSTR_JL = 23;
		private const Int32 INSTR_JGE = 24;
		private const Int32 INSTR_JLE = 25;

		private const Int32 INSTR_PUSH = 26;
		private const Int32 INSTR_POP = 27;

		private const Int32 INSTR_CALL = 28;
		private const Int32 INSTR_RET = 29;
		private const Int32 INSTR_CALLHOST = 30;

		private const Int32 INSTR_PAUSE = 31;
		private const Int32 INSTR_EXIT = 32;

		// ---- Stack -----------------------------------------------------------------------------

		private const Int32 DEF_STACK_SIZE = 1024;// The default stack size

		// ---- Coercion --------------------------------------------------------------------------

		private const Int32 MAX_COERCION_STRING_SIZE = 64;          // The maximum allocated space for a
																	// string coercion

		// ---- Multithreading --------------------------------------------------------------------

		private const Int32 THREAD_MODE_MULTI = 0;          // Multithreaded execution
		private const Int32 THREAD_MODE_SINGLE = 1;          // Single-threaded execution

		private const Int32 THREAD_PRIORITY_DUR_LOW = 20;          // Low-priority thread timeslice
		private const Int32 THREAD_PRIORITY_DUR_MED = 40;          // Medium-priority thread timeslice
		private const Int32 THREAD_PRIORITY_DUR_HIGH = 80;          // High-priority thread timeslice

		// ---- The Host API ----------------------------------------------------------------------

		private const Int32 MAX_HOST_API_SIZE = 1024;        // Maximum number of functions in the
															 // host API

		// ---- Functions -------------------------------------------------------------------------

		private const Int32 MAX_FUNC_NAME_SIZE = 256;         // Maximum size of a function's name

		// ---- Data Structures -----------------------------------------------------------------------

		// ---- Runtime Value ---------------------------------------------------------------------
		//[StructLayout (LayoutKind.Explicit, CharSet = CharSet.Ansi, Size = 16)]
		[Serializable]
		class Value                           // A runtime value
		{

			//[FieldOffset (0)]
			public Int32 iType;                                  // Type

			//[FieldOffset (4)]
			public Int32 iIntValue;                        // Int32eger literal
														   //[FieldOffset (4)]
			public float fFloatLiteral;                    // Float literal										         

			////[FieldOffset (4)]
			//public Int32 iIntValue;                        // Stack Index
			//												 //[FieldOffset (4)]
			//public Int32 iIntValue;                        // Instruction index
			//												 //[FieldOffset (4)]
			//public Int32 iIntValue;                         // Function index
			//[FieldOffset (4)]
			public Int32 iHostAPICallIndex;                  // Host API Call index
															 //[FieldOffset (4)]
															 //public Int32 iIntValue;                               // Register code

			//[FieldOffset (8)]
			public Int32 iOffsetIndex;                           // Index of the offset

			//[FieldOffset (12)]
			//[MarshalAs (UnmanagedType.LPStr)]
			public string pstrStringLiteral;             // String literal

			public void SetValue (ref Value value)
			{
				this.iType = value.iType;
				this.iIntValue = value.iIntValue;
				this.fFloatLiteral = value.fFloatLiteral;
				//this.iIntValue = value.iIntValue;
				//this.iIntValue = value.iIntValue;
				//this.iIntValue = value.iIntValue;
				this.iHostAPICallIndex = value.iHostAPICallIndex;
				//this.iIntValue = value.iIntValue;
				this.iOffsetIndex = value.iOffsetIndex;
			}
		}

		// ---- Runtime Stack ---------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct RuntimeStack                    // A runtime stack
		{

			public Value [] pElmnts;                         // The stack elements of Value
			public Int32 iSize;                                  // The number of elements in the stack

			public Int32 iTopIndex;                              // The top index
			public Int32 iFrameIndex;                            // Index of the top of the current
																 // stack frame.
		}

		// ---- Functions -------------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]

		struct Func                            // A function
		{
			public Int32 iEntryPoint;                            // The entry poInt32
			public Int32 iParamCount;                            // The parameter count
			public Int32 iLocalDataSize;                         // Total size of all local data
			public Int32 iStackFrameSize;                        // Total size of the stack frame
																 //[MarshalAs (UnmanagedType.ByValTStr, SizeConst = MAX_FUNC_NAME_SIZE + 1)]
			public string pstrName;   // The function's name
		}


		// ---- Instructions ----------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct Instr                           // An instruction
		{
			public Int32 iOpcode;                                // The opcode
			public Int32 iOpCount;                               // The number of operands
			public Value [] pOpList;                            // The operand list of Values
		}

		//[StructLayout (LayoutKind.Sequential)]
		struct InstrStream                     // An instruction stream
		{
			public Instr [] pInstrs;                         // The instructions themselves
			public Int32 iSize;                                  // The number of instructions in the
																 // stream
			public Int32 iCurrInstr;                             // The instruction poInt32er
		}


		// ---- Function Table --------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct FuncTable                       // A function table
		{
			public Func [] pFuncs;                              // PoInt32er to the function array
			public Int32 iSize;                                  // The number of functions in the array
		}
		//FuncTable;

		// ---- Host API Call Table ---------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct HostAPICallTable                // A host API call table
		{
			public string [] ppstrCalls;                          // PoInt32er to the call array
			public Int32 iSize;                                  // The number of calls in the array
		}
		//HostAPICallTable;

		// ---- Scripts ---------------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct Script                          // Encapsulates a full script
		{
			public bool iIsActive;                              // Is this script structure in use?

			// Header data

			public Int32 iGlobalDataSize;                        // The size of the script's global data
			public bool iIsMainFuncPresent;                      // Is _Main () present?
			public Int32 iMainFuncIndex;                         // _Main ()'s function index

			// Runtime tracking

			public bool iIsRunning;                              // Is the script running?
			public bool iIsPaused;                               // Is the script currently paused?
			public Int32 iPauseEndTime;                          // If so, when should it resume?

			// Threading

			public Int32 iTimesliceDur;                          // The thread's timeslice duration

			// Register file

			public Value _RetVal;                              // The _RetVal register

			// Script data

			public InstrStream InstrStream;                    // The instruction stream
			public RuntimeStack Stack;                         // The runtime stack
			public FuncTable FuncTable;                        // The function table
			public HostAPICallTable HostAPICallTable;          // The host API call table


		}
		//Script;

		// ---- Host API --------------------------------------------------------------------------
		//[StructLayout (LayoutKind.Sequential)]
		struct HostAPIFunc                     // Host API function
		{
			public bool iIsActive;                              // Is this slot in use?

			public Int32 iThreadIndex;                           // The thread to which this function
																 // is visible
			public string pstrName;                            // The function name
			public HostAPIFuncPntr fnFunc;                     // PoInt32er to the function definition
		}
		//HostAPIFunc;

		// ---- Globals -------------------------------------------------------------------------------

		// ---- Scripts ---------------------------------------------------------------------------

		Script [] g_Scripts = new Script [MAX_THREAD_COUNT];            // The script array

		// ---- Threading -------------------------------------------------------------------------

		Int32 g_iCurrThreadMode;                          // The current threading mode
		Int32 g_iCurrThread;                              // The currently running thread
		Int32 g_iCurrThreadActiveTime;                    // The time at which the current thread
														  // was activated

		// ---- The Host API ----------------------------------------------------------------------

		HostAPIFunc [] g_HostAPI = new HostAPIFunc [MAX_HOST_API_SIZE];    // The host API

		// ----- String List ----------------------------------------------------------------------

		List<string> g_StringList = new List<string> ();            //Storage all string

		// ---- Macros --------------------------------------------------------------------------------

		/******************************************************************************************
		*
		*	ResolveStackIndex ()
		*
		*	Resolves a stack index by translating negative indices relative to the top of the
		*	stack, to positive ones relative to the bottom.
		*/

		int ResolveStackIndex (int iIndex) => iIndex < 0 ? iIndex += g_Scripts [g_iCurrThread].Stack.iFrameIndex : iIndex;

		/******************************************************************************************
		*
		*   IsValidThreadIndex ()
		*
		*   Returns true if the specified thread index is within the bounds of the array, false
		*   otherwise.
		*/

		bool IsValidThreadIndex (int iIndex) => iIndex < 0 || iIndex > MAX_THREAD_COUNT ? false : true;

		/******************************************************************************************
		*
		*   IsThreadActive ()
		*
		*   Returns true if the specified thread is both a valid index and active, false otherwise.
		*/

		bool IsThreadActive (int iIndex) => IsValidThreadIndex (iIndex) && g_Scripts [iIndex].iIsActive;




		public void XS_Init ()
		{
			// ---- Initialize the script array

			for (int iCurrScriptIndex = 0; iCurrScriptIndex < MAX_THREAD_COUNT; ++iCurrScriptIndex) {
				g_Scripts [iCurrScriptIndex].iIsActive = false;

				g_Scripts [iCurrScriptIndex].iIsRunning = false;
				g_Scripts [iCurrScriptIndex].iIsMainFuncPresent = false;
				g_Scripts [iCurrScriptIndex].iIsPaused = false;


				g_Scripts [iCurrScriptIndex].InstrStream.pInstrs = null;
				g_Scripts [iCurrScriptIndex].Stack.pElmnts = null;
				g_Scripts [iCurrScriptIndex].FuncTable.pFuncs = null;
				g_Scripts [iCurrScriptIndex].HostAPICallTable.ppstrCalls = null;
			}

			// ---- Initialize the host API

			for (int iCurrHostAPIFunc = 0; iCurrHostAPIFunc < MAX_HOST_API_SIZE; ++iCurrHostAPIFunc) {
				g_HostAPI [iCurrHostAPIFunc].iIsActive = false;
				g_HostAPI [iCurrHostAPIFunc].pstrName = null;
			}

			// ---- Set up the threads

			g_iCurrThreadMode = THREAD_MODE_MULTI;
			g_iCurrThread = 0;
		}

		/******************************************************************************************
		*
		*	XS_ShutDown ()
		*
		*	Shuts down the runtime environment.
		*/

		public void XS_ShutDown ()
		{
			// ---- Unload any scripts that may still be in memory

			for (int iCurrScriptIndex = 0; iCurrScriptIndex < MAX_THREAD_COUNT; ++iCurrScriptIndex)
				XS_UnloadScript (iCurrScriptIndex);

			// ---- Free the host API's function name strings

			for (int iCurrHostAPIFunc = 0; iCurrHostAPIFunc < MAX_HOST_API_SIZE; ++iCurrHostAPIFunc)
				//if (g_HostAPI [iCurrHostAPIFunc].pstrName != null)
				g_HostAPI [iCurrHostAPIFunc].pstrName = null;
		}

		/// <summary>
		/// Reads the string from file stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="dest">Destination.</param>
		/// <param name="size">Size.</param>
		void ReadStringFromFileStream (BinaryReader stream, ref string dest, int size = 4)
		{
			Byte [] stackSize = new Byte [size];
			stream.Read (stackSize, 0, size);
			//stackSize [size] = (Byte)'\0';
			//dest = System.Text.Encoding.ASCII.GetString (stackSize);
			dest = System.Text.Encoding.UTF8.GetString (stackSize);
		}

		/// <summary>
		/// Reads the int from file stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="dest">Destination.</param>
		/// <param name="size">Size.</param>
		void ReadIntFromFileStream (BinaryReader stream, ref int dest, int size = sizeof (Int32))
		{
			Byte [] stackSize = new Byte [4];
			stream.Read (stackSize, 0, size);
			dest = BitConverter.ToInt32 (stackSize, 0);
		}

		/// <summary>
		/// Reads the float from file stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="dest">Destination.</param>
		/// <param name="size">Size.</param>
		void ReadFloatFromFileStream (BinaryReader stream, ref float dest, int size = sizeof (Single))
		{
			Byte [] stackSize = new Byte [4];
			stream.Read (stackSize, 0, size);
			dest = BitConverter.ToSingle (stackSize, 0);
		}

		/// <summary>
		/// Reads the bool from file stream.
		/// </summary>
		/// <param name="stream">Stream.</param>
		/// <param name="dest">If set to <c>true</c> destination.</param>
		void ReadBoolFromFileStream (BinaryReader stream, ref bool dest)
		{
			Byte [] stackSize = new Byte [1];
			stream.Read (stackSize, 0, 1);
			dest = BitConverter.ToBoolean (stackSize, 0);
		}

		/// <summary>
		/// Deeps copy object.
		/// </summary>
		/// <returns>The copy.</returns>
		/// <param name="obj">Object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T DeepCopy<T> (T obj)
		{
			object retval;
			using (MemoryStream ms = new MemoryStream ()) {
				BinaryFormatter bf = new BinaryFormatter ();
				//序列化成流
				bf.Serialize (ms, obj);
				ms.Seek (0, SeekOrigin.Begin);
				//反序列化成对象
				retval = bf.Deserialize (ms);
				ms.Close ();
			}
			return (T)retval;
		}


		/******************************************************************************************
		*
		*	XS_LoadScript ()
		*
		*	Loads an .XSE file into memory.
		*/

		public int XS_LoadScript (string pstrFilename, ref int iThreadIndex, int iThreadTimeslice)
		{
			// ---- Find the next free script index

			bool iFreeThreadFound = false;
			for (int iCurrThreadIndex = 0; iCurrThreadIndex < MAX_THREAD_COUNT; ++iCurrThreadIndex) {
				// If the current thread is not in use, use it

				if (!g_Scripts [iCurrThreadIndex].iIsActive) {
					iThreadIndex = iCurrThreadIndex;
					iFreeThreadFound = true;
					break;
				}
			}

			// If a thread wasn't found, return an out of threads error

			if (!iFreeThreadFound)
				return XS_LOAD_ERROR_OUT_OF_THREADS;


			Stream stream;
			if (Application.platform == RuntimePlatform.Android) {

				TextAsset asset = Resources.Load (pstrFilename) as TextAsset;
				stream = new MemoryStream (asset.bytes);


			}
			if (Application.platform == RuntimePlatform.WindowsPlayer) {

				TextAsset asset = Resources.Load (pstrFilename) as TextAsset;
				stream = new MemoryStream (asset.bytes);


			}
			if (Application.platform == RuntimePlatform.OSXPlayer) {
				TextAsset asset = Resources.Load (pstrFilename) as TextAsset;
				stream = new MemoryStream (asset.bytes);
			} else {
				TextAsset asset = Resources.Load (pstrFilename) as TextAsset;
				stream = new MemoryStream (asset.bytes);

			}

			// ---- Open the input file

			BinaryReader pScriptFile = new BinaryReader (stream);
			if (pScriptFile == null)
				return XS_LOAD_ERROR_FILE_IO;

			// ---- Read the header

			// Create a buffer to hold the file's ID string (4 bytes + 1 null terminator = 5)

			Byte [] pstrIDString = new Byte [5];
			if (pstrIDString == null)
				return XS_LOAD_ERROR_OUT_OF_MEMORY;

			// Read the string (4 bytes) and append a null terminator

			string exeIdString = "";
			ReadStringFromFileStream (pScriptFile, ref exeIdString, 4);


			// Compare the data read from the file to the ID string and exit on an error if they don't
			// match
			// ignore culture case for a better speed of compare

			if (string.Compare (XSE_ID_STRING, exeIdString, StringComparison.Ordinal) != 0)
				return XS_LOAD_ERROR_INVALID_XSE;

			// Free the buffer

			//free (pstrIDString);



			// Read the script version (2 bytes total)

			int iMajorVersion = 0;
			int iMinorVersion = 0;

			iMajorVersion = pScriptFile.ReadByte ();
			iMinorVersion = pScriptFile.ReadByte ();

			// Validate the version, since this prototype only supports version 0.8 scripts

			if (iMajorVersion != 0 || iMinorVersion != 8)
				return XS_LOAD_ERROR_UNSUPPORTED_VERS;

			// Read the stack size (4 bytes)

			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].Stack.iSize);

			// Check for a default stack size request

			if (g_Scripts [iThreadIndex].Stack.iSize == 0)
				g_Scripts [iThreadIndex].Stack.iSize = DEF_STACK_SIZE;

			// Allocate the runtime stack

			int iStackSize = g_Scripts [iThreadIndex].Stack.iSize;
			g_Scripts [iThreadIndex].Stack.pElmnts = new Value [iStackSize];
			for (int i = 0; i < iStackSize; i++) g_Scripts [iThreadIndex].Stack.pElmnts [i] = new Value ();

			if (g_Scripts [iThreadIndex].Stack.pElmnts == null)
				return XS_LOAD_ERROR_OUT_OF_MEMORY;

			// Read the global data size (4 bytes)

			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].iGlobalDataSize);

			// Check for presence of _Main () (1 byte)

			ReadBoolFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].iIsMainFuncPresent);

			// Read _Main ()'s function index (4 bytes)

			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].iMainFuncIndex);


			// Read the priority type (1 byte)

			int iPriorityType = 0;
			iPriorityType = pScriptFile.ReadByte ();

			// Read the user-defined priority (4 bytes)

			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].iTimesliceDur);


			// Override the script-specified priority if necessary

			if (iThreadTimeslice != XS_THREAD_PRIORITY_USER)
				iPriorityType = iThreadTimeslice;

			// If the priority type is not set to user-defined, fill in the appropriate timeslice
			// duration

			switch (iPriorityType) {
			case XS_THREAD_PRIORITY_LOW:
				g_Scripts [iThreadIndex].iTimesliceDur = THREAD_PRIORITY_DUR_LOW;
				break;

			case XS_THREAD_PRIORITY_MED:
				g_Scripts [iThreadIndex].iTimesliceDur = THREAD_PRIORITY_DUR_MED;
				break;

			case XS_THREAD_PRIORITY_HIGH:
				g_Scripts [iThreadIndex].iTimesliceDur = THREAD_PRIORITY_DUR_HIGH;
				break;
			}

			// ---- Read the instruction stream

			// Read the instruction count (4 bytes)

			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].InstrStream.iSize);




			// Allocate the stream

			g_Scripts [iThreadIndex].InstrStream.pInstrs = new Instr [g_Scripts [iThreadIndex].InstrStream.iSize];

			if (g_Scripts [iThreadIndex].InstrStream.pInstrs == null)
				return XS_LOAD_ERROR_OUT_OF_MEMORY;

			// Read the instruction data

			for (int iCurrInstrIndex = 0; iCurrInstrIndex < g_Scripts [iThreadIndex].InstrStream.iSize; ++iCurrInstrIndex) {
				// Read the opcode (2 bytes)

				g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpcode = 0;
				//fread (&g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpcode, 2, 1, pScriptFile);
				ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpcode, 2);

				// Read the operand count (1 byte)

				g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpCount = 0;
				ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpCount, 1);

				int iOpCount = g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpCount;

				// Allocate space for the operand list in a temporary pointer

				Value [] pOpList = new Value [iOpCount];
				for (int i = 0; i < iOpCount; i++) pOpList [i] = new Value ();

				if (pOpList == null)
					return XS_LOAD_ERROR_OUT_OF_MEMORY;

				// Read in the operand list (N bytes)

				for (int iCurrOpIndex = 0; iCurrOpIndex < iOpCount; ++iCurrOpIndex) {
					// Read in the operand type (1 byte)

					pOpList [iCurrOpIndex].iType = 0;
					ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iType, 1);
					//fread (&pOpList [iCurrOpIndex].iType, 1, 1, pScriptFile);

					// Depending on the type, read in the operand data

					switch (pOpList [iCurrOpIndex].iType) {
					// Integer literal

					case OP_TYPE_INT:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));
						//fread (&pOpList [iCurrOpIndex].iIntLiteral, sizeof (int), 1, pScriptFile);
						break;

					// Floating-point literal

					case OP_TYPE_FLOAT:
						ReadFloatFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].fFloatLiteral, sizeof (float));
						//fread (&pOpList [iCurrOpIndex].fFloatLiteral, sizeof (float), 1, pScriptFile);
						break;

					// String index

					case OP_TYPE_STRING:

						// Since there's no field in the Value structure for string table
						// indices, read the index into the integer literal field and set
						// its type to string index
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));
						pOpList [iCurrOpIndex].iType = OP_TYPE_STRING;
						break;

					// Instruction index

					case OP_TYPE_INSTR_INDEX:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));

						break;

					// Absolute stack index

					case OP_TYPE_ABS_STACK_INDEX:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));

						//fread (&pOpList [iCurrOpIndex].iStackIndex, sizeof (int), 1, pScriptFile);
						break;

					// Relative stack index

					case OP_TYPE_REL_STACK_INDEX:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iOffsetIndex, sizeof (int));

						//fread (&pOpList [iCurrOpIndex].iStackIndex, sizeof (int), 1, pScriptFile);
						//fread (&pOpList [iCurrOpIndex].iOffsetIndex, sizeof (int), 1, pScriptFile);
						break;

					// Function index

					case OP_TYPE_FUNC_INDEX:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));

						//fread (&pOpList [iCurrOpIndex].iFuncIndex, sizeof (int), 1, pScriptFile);
						break;

					// Host API call index

					case OP_TYPE_HOST_API_CALL_INDEX:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iHostAPICallIndex, sizeof (int));

						//fread (&pOpList [iCurrOpIndex].iHostAPICallIndex, sizeof (int), 1, pScriptFile);
						break;

					// Register

					case OP_TYPE_REG:
						ReadIntFromFileStream (pScriptFile, ref pOpList [iCurrOpIndex].iIntValue, sizeof (int));

						//fread (&pOpList [iCurrOpIndex].iReg, sizeof (int), 1, pScriptFile);
						break;
					}
				}

				// Assign the operand list pointer to the instruction stream

				g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].pOpList = pOpList;
			}


			// ---- Read the string table

			// Read the table size (4 bytes)

			int iStringTableSize = 0;
			ReadIntFromFileStream (pScriptFile, ref iStringTableSize, sizeof (int));

			// If the string table exists, read it

			if (iStringTableSize != 0) {
				// Allocate a string table of this size

				string [] ppstrStringTable = new string [iStringTableSize];
				if (ppstrStringTable == null)
					return XS_LOAD_ERROR_OUT_OF_MEMORY;
				//for(int i = 0; i <iStringTableSize; i++) 

				// Read in each string

				for (int iCurrStringIndex = 0; iCurrStringIndex < iStringTableSize; ++iCurrStringIndex) {
					// Read in the string size (4 bytes)

					int iStringSize = 0;
					ReadIntFromFileStream (pScriptFile, ref iStringSize, sizeof (int));


					// Allocate space for the string plus a null terminator
					// Read in the string data (N bytes) and append the null terminator

					string pstrCurrString = "";
					ReadStringFromFileStream (pScriptFile, ref pstrCurrString, iStringSize);


					// Assign the string pointer to the string table

					ppstrStringTable [iCurrStringIndex] = pstrCurrString;
				}

				// Run through each operand in the instruction stream and assign copies of string
				// operand's corresponding string literals

				for (int iCurrInstrIndex = 0; iCurrInstrIndex < g_Scripts [iThreadIndex].InstrStream.iSize; ++iCurrInstrIndex) {
					// Get the instruction's operand count and a copy of it's operand list

					int iOpCount = g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpCount;
					Value [] pOpList = g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].pOpList;

					// Loop through each operand

					for (int iCurrOpIndex = 0; iCurrOpIndex < iOpCount; ++iCurrOpIndex) {
						// If the operand is a string index, make a local copy of it's corresponding
						// string in the table

						if (pOpList [iCurrOpIndex].iType == OP_TYPE_STRING) {
							// Get the string index from the operand's integer literal field

							int iStringIndex = pOpList [iCurrOpIndex].iIntValue;

							// Allocate a new string to hold a copy of the one in the table

							//string pstrStringCopy;
							//if (!(pstrStringCopy = (string)malloc (strlen () + 1)))
							//return XS_LOAD_ERROR_OUT_OF_MEMORY;

							// Make a copy of the string

							//strcpy (pstrStringCopy, ppstrStringTable [iStringIndex]);

							//int stringListIndex = g_StringList.Count;
							//g_StringList.Add (ppstrStringTable [iStringIndex]);

							// Save the string pointer in the operand list

							pOpList [iCurrOpIndex].pstrStringLiteral = ppstrStringTable [iStringIndex];
						}
					}
				}

				// ---- Free the original strings

				for (int iCurrStringIndex = 0; iCurrStringIndex < iStringTableSize; ++iCurrStringIndex)
					ppstrStringTable [iCurrStringIndex] = null;

				ppstrStringTable = null;
				// ---- Free the string table itself

				//free (ppstrStringTable);
			}


			// ---- Read the function table

			// Read the function count (4 bytes)

			int iFuncTableSize = 0;
			ReadIntFromFileStream (pScriptFile, ref iFuncTableSize, 4);
			//fread (&iFuncTableSize, 4, 1, pScriptFile);

			g_Scripts [iThreadIndex].FuncTable.iSize = iFuncTableSize;

			// Allocate the table
			g_Scripts [iThreadIndex].FuncTable.pFuncs = new Func [iFuncTableSize];
			//for (int i = 0; i < iFuncTableSize) ; i++) 
			if (g_Scripts [iThreadIndex].FuncTable.pFuncs == null)
				return XS_LOAD_ERROR_OUT_OF_MEMORY;

			// Read each function

			for (int iCurrFuncIndex = 0; iCurrFuncIndex < iFuncTableSize; ++iCurrFuncIndex) {
				// Read the entry point (4 bytes)

				int iEntryPoint = -1;
				ReadIntFromFileStream (pScriptFile, ref iEntryPoint, 4);

				// Read the parameter count (1 byte)

				int iParamCount = 0;
				ReadIntFromFileStream (pScriptFile, ref iParamCount, 1);

				// Read the local data size (4 bytes)

				int iLocalDataSize = 0;
				ReadIntFromFileStream (pScriptFile, ref iLocalDataSize, 4);


				// Calculate the stack size

				int iStackFrameSize = iParamCount + 1 + iLocalDataSize;

				// Read the function name length (1 byte)

				int iFuncNameLength = 0;
				ReadIntFromFileStream (pScriptFile, ref iFuncNameLength, 1);



				// Read the function name (N bytes) and append a null-terminator
				ReadStringFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].pstrName, iFuncNameLength);
				//fread (&g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].pstrName, iFuncNameLength, 1, pScriptFile);
				//g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].pstrName [iFuncNameLength] = '\0';

				// Write everything to the function table

				g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].iEntryPoint = iEntryPoint;
				g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].iParamCount = iParamCount;
				g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].iLocalDataSize = iLocalDataSize;
				g_Scripts [iThreadIndex].FuncTable.pFuncs [iCurrFuncIndex].iStackFrameSize = iStackFrameSize;
			}

			// ---- Read the host API call table

			// Read the host API call count
			ReadIntFromFileStream (pScriptFile, ref g_Scripts [iThreadIndex].HostAPICallTable.iSize, 4);
			//fread (&g_Scripts [iThreadIndex].HostAPICallTable.iSize, 4, 1, pScriptFile);

			// Allocate the table
			g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls = new string [g_Scripts [iThreadIndex].HostAPICallTable.iSize];
			if (g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls == null)
				return XS_LOAD_ERROR_OUT_OF_MEMORY;

			// Read each host API call

			for (int iCurrCallIndex = 0; iCurrCallIndex < g_Scripts [iThreadIndex].HostAPICallTable.iSize; ++iCurrCallIndex) {
				// Read the host API call string size (1 byte)

				int iCallLength = 0;
				ReadIntFromFileStream (pScriptFile, ref iCallLength, 1);
				//fread (&iCallLength, 1, 1, pScriptFile);

				// Allocate space for the string plus the null terminator in a temporary pointer

				string pstrCurrCall = "";
				ReadStringFromFileStream (pScriptFile, ref pstrCurrCall, iCallLength);
				//if (!(pstrCurrCall = (string)malloc (iCallLength + 1)))
				//return XS_LOAD_ERROR_OUT_OF_MEMORY;

				// Read the host API call string data and append the null terminator

				//fread (pstrCurrCall, iCallLength, 1, pScriptFile);
				//pstrCurrCall [iCallLength] = '\0';

				// Assign the temporary pointer to the table

				g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls [iCurrCallIndex] = pstrCurrCall;
			}

			// ---- Close the input file
			pScriptFile.Close ();





			// The script is fully loaded and ready to go, so set the active flag

			g_Scripts [iThreadIndex].iIsActive = true;

			// Reset the script

			XS_ResetScript (iThreadIndex);

			// Return a success code

			return XS_LOAD_OK;
		}


		/******************************************************************************************
		*
		*	XS_UnloadScript ()
		*
		*	Unloads a script from memory.
		*/

		void XS_UnloadScript (int iThreadIndex)
		{
			// Exit if the script isn't active

			if (!g_Scripts [iThreadIndex].iIsActive)
				return;

			// ---- Free The instruction stream

			// First check to see if any instructions have string operands, and free them if they
			// do

			for (int iCurrInstrIndex = 0; iCurrInstrIndex < g_Scripts [iThreadIndex].InstrStream.iSize; ++iCurrInstrIndex) {
				// Make a local copy of the operand count and operand list

				int iOpCount = g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].iOpCount;
				Value [] pOpList = g_Scripts [iThreadIndex].InstrStream.pInstrs [iCurrInstrIndex].pOpList;

				// Loop through each operand and free its string pointer

				for (int iCurrOpIndex = 0; iCurrOpIndex < iOpCount; ++iCurrOpIndex)

					pOpList [iCurrOpIndex].pstrStringLiteral = null;
			}

			// Now free the stream itself

			if (g_Scripts [iThreadIndex].InstrStream.pInstrs != null)
				g_Scripts [iThreadIndex].InstrStream.pInstrs = null;

			// ---- Free the runtime stack

			// Free any strings that are still on the stack

			for (int iCurrElmtnIndex = 0; iCurrElmtnIndex < g_Scripts [iThreadIndex].Stack.iSize; ++iCurrElmtnIndex)
				if (g_Scripts [iThreadIndex].Stack.pElmnts [iCurrElmtnIndex].iType == OP_TYPE_STRING)
					g_Scripts [iThreadIndex].Stack.pElmnts [iCurrElmtnIndex].pstrStringLiteral = null;

			// Now free the stack itself

			//if (g_Scripts [iThreadIndex].Stack.pElmnts != null)
			g_Scripts [iThreadIndex].Stack.pElmnts = null;


			// ---- Free the function table

			g_Scripts [iThreadIndex].FuncTable.pFuncs = null;


			// --- Free the host API call table

			// First free each string in the table individually

			for (int iCurrCallIndex = 0; iCurrCallIndex < g_Scripts [iThreadIndex].HostAPICallTable.iSize; ++iCurrCallIndex)
				if (g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls [iCurrCallIndex] != null)
					g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls [iCurrCallIndex] = null;

			// Now free the table itself
			g_Scripts [iThreadIndex].HostAPICallTable.ppstrCalls = null;
		}

		/******************************************************************************************
		*
		*	XS_ResetScript ()
		*
		*	Resets the script. This function accepts a thread index rather than relying on the
		*	currently active thread, because scripts can (and will) need to be reset arbitrarily.
		*/

		void XS_ResetScript (int iThreadIndex)
		{
			// Get _Main ()'s function index in case we need it

			int iMainFuncIndex = g_Scripts [iThreadIndex].iMainFuncIndex;

			// If the function table is present, set the entry point

			if (g_Scripts [iThreadIndex].FuncTable.pFuncs != null) {
				// If _Main () is present, read _Main ()'s index of the function table to get its
				// entry point

				if (g_Scripts [iThreadIndex].iIsMainFuncPresent) {
					g_Scripts [iThreadIndex].InstrStream.iCurrInstr = g_Scripts [iThreadIndex].FuncTable.pFuncs [iMainFuncIndex].iEntryPoint;
				}
			}

			// Clear the stack

			g_Scripts [iThreadIndex].Stack.iTopIndex = 0;
			g_Scripts [iThreadIndex].Stack.iFrameIndex = 0;

			// Set the entire stack to null

			for (int iCurrElmntIndex = 0; iCurrElmntIndex < g_Scripts [iThreadIndex].Stack.iSize; ++iCurrElmntIndex)
				g_Scripts [iThreadIndex].Stack.pElmnts [iCurrElmntIndex].iType = OP_TYPE_NULL;

			// Unpause the script

			g_Scripts [iThreadIndex].iIsPaused = false;

			// Allocate space for the globals

			PushFrame (iThreadIndex, g_Scripts [iThreadIndex].iGlobalDataSize);

			// If _Main () is present, push its stack frame (plus one extra stack element to
			// compensate for the function index that usually sits on top of stack frames and
			// causes indices to start from -2)

			PushFrame (iThreadIndex, g_Scripts [iThreadIndex].FuncTable.pFuncs [iMainFuncIndex].iLocalDataSize + 1);
		}

		/******************************************************************************************
		*
		*	XS_RunScripts ()
		*
		*	Runs the currenty loaded script array for a given timeslice duration.
		*/

		public void XS_RunScripts (int iTimesliceDur)
		{
			// Begin a loop that runs until a keypress. The instruction pointer has already been
			// initialized with a prior call to ResetScripts (), so execution can begin

			// Create a flag that instructions can use to break the execution loop

			bool iExitExecLoop = false;

			// Create a variable to hold the time at which the main timeslice started

			int iMainTimesliceStartTime = GetCurrTime ();

			// Create a variable to hold the current time

			int iCurrTime;

			while (true) {
				// Check to see if all threads have terminated, and if so, break the execution
				// cycle

				bool iIsStillActive = false;
				for (int iCurrThreadIndex = 0; iCurrThreadIndex < MAX_THREAD_COUNT; ++iCurrThreadIndex) {
					if (g_Scripts [iCurrThreadIndex].iIsActive && g_Scripts [iCurrThreadIndex].iIsRunning)
						iIsStillActive = true;
				}
				if (!iIsStillActive)
					break;

				// Update the current time

				iCurrTime = GetCurrTime ();

				// Check for a context switch if the threading mode is set for multithreading

				if (g_iCurrThreadMode == THREAD_MODE_MULTI) {
					// If the current thread's timeslice has elapsed, or if it's terminated switch
					// to the next valid thread

					if (iCurrTime > g_iCurrThreadActiveTime + g_Scripts [g_iCurrThread].iTimesliceDur ||
						 !g_Scripts [g_iCurrThread].iIsRunning) {
						// Loop until the next thread is found

						while (true) {
							// Move to the next thread in the array

							++g_iCurrThread;

							// If we're past the end of the array, loop back around

							if (g_iCurrThread >= MAX_THREAD_COUNT)
								g_iCurrThread = 0;

							// If the thread we've chosen is active and running, break the loop

							if (g_Scripts [g_iCurrThread].iIsActive && g_Scripts [g_iCurrThread].iIsRunning)
								break;
						}

						// Reset the timeslice

						g_iCurrThreadActiveTime = iCurrTime;
					}
				}

				// Is the script currently paused?

				if (g_Scripts [g_iCurrThread].iIsPaused) {
					// Has the pause duration elapsed yet?

					if (iCurrTime >= g_Scripts [g_iCurrThread].iPauseEndTime) {
						// Yes, so unpause the script

						g_Scripts [g_iCurrThread].iIsPaused = false;
					} else {
						// No, so skip this iteration of the execution cycle

						continue;
					}
				}

				// Make a copy of the instruction pointer to compare later

				int iCurrInstr = g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

				// Get the current opcode

				int iOpcode = g_Scripts [g_iCurrThread].InstrStream.pInstrs [iCurrInstr].iOpcode;

				// Execute the current instruction based on its opcode, as long as we aren't
				// currently paused

				switch (iOpcode) {
				// ---- Binary Operations

				// All of the binary operation instructions (move, arithmetic, and bitwise)
				// are combined into a single case that keeps us from having to rewrite the
				// otherwise redundant operand resolution and result storage phases over and
				// over. We then use an additional switch block to determine which operation
				// should be performed.

				// Move

				case INSTR_MOV:

				// Arithmetic Operations

				case INSTR_ADD:
				case INSTR_SUB:
				case INSTR_MUL:
				case INSTR_DIV:
				case INSTR_MOD:
				case INSTR_EXP:

				// Bitwise Operations

				case INSTR_AND:
				case INSTR_OR:
				case INSTR_XOR:
				case INSTR_SHL:
				case INSTR_SHR: {
						// Get a local copy of the destination operand (operand index 0)

						Value Dest = ResolveOpValue (0);

						// Get a local copy of the source operand (operand index 1)

						Value Source = ResolveOpValue (1);

						// Depending on the instruction, perform a binary operation

						switch (iOpcode) {
						// Move

						case INSTR_MOV:

							// Skip cases where the two operands are the same

							if (ResolveOpPntr (0) == ResolveOpPntr (1))
								break;

							// Copy the source operand into the destination

							CopyValue (ref Dest, Source);

							break;

						// The arithmetic instructions only work with destination types that
						// are either integers or floats. They first check for integers and
						// assume that anything else is a float. Mod only works with integers.

						// Add

						case INSTR_ADD:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue += ResolveOpAsInt (1);
							else
								Dest.fFloatLiteral += ResolveOpAsFloat (1);

							break;

						// Subtract

						case INSTR_SUB:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue -= ResolveOpAsInt (1);
							else
								Dest.fFloatLiteral -= ResolveOpAsFloat (1);

							break;

						// Multiply

						case INSTR_MUL:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue *= ResolveOpAsInt (1);
							else
								Dest.fFloatLiteral *= ResolveOpAsFloat (1);

							break;

						// Divide

						case INSTR_DIV:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue /= ResolveOpAsInt (1);
							else
								Dest.fFloatLiteral /= ResolveOpAsFloat (1);

							break;

						// Modulus

						case INSTR_MOD:

							// Remember, Mod works with integers only

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue %= ResolveOpAsInt (1);

							break;

						// Exponentiate

						case INSTR_EXP:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue = (int)Math.Pow (Dest.iIntValue, ResolveOpAsInt (1));
							else
								Dest.fFloatLiteral = (float)Math.Pow (Dest.fFloatLiteral, ResolveOpAsFloat (1));

							break;

						// The bitwise instructions only work with integers. They do nothing
						// when the destination data type is anything else.

						// And

						case INSTR_AND:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue &= ResolveOpAsInt (1);

							break;

						// Or

						case INSTR_OR:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue |= ResolveOpAsInt (1);

							break;

						// Exclusive Or

						case INSTR_XOR:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue ^= ResolveOpAsInt (1);

							break;

						// Shift Left

						case INSTR_SHL:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue <<= ResolveOpAsInt (1);

							break;

						// Shift Right

						case INSTR_SHR:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue >>= ResolveOpAsInt (1);

							break;
						}

						// Use ResolveOpPntr () to get a pointer to the destination Value structure and
						// move the result there

						Value value = ResolveOpPntr (0);
						value.SetValue (ref Dest);

						break;
					}

				// ---- Unary Operations

				// These instructions work much like the binary operations in the sense that
				// they only work with integers and floats (except Not, which works with
				// integers only). Any other destination data type will be ignored.

				case INSTR_NEG:
				case INSTR_NOT:
				case INSTR_INC:
				case INSTR_DEC: {
						// Get the destination type (operand index 0)

						int iDestStoreType = GetOpType (0);

						// Get a local copy of the destination itself

						Value Dest = ResolveOpValue (0);

						switch (iOpcode) {
						// Negate

						case INSTR_NEG:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue = -Dest.iIntValue;
							else
								Dest.fFloatLiteral = -Dest.fFloatLiteral;

							break;

						// Not

						case INSTR_NOT:

							if (Dest.iType == OP_TYPE_INT)
								Dest.iIntValue = ~Dest.iIntValue;

							break;

						// Increment

						case INSTR_INC:

							if (Dest.iType == OP_TYPE_INT)
								++Dest.iIntValue;
							else
								++Dest.fFloatLiteral;

							break;

						// Decrement

						case INSTR_DEC:

							if (Dest.iType == OP_TYPE_INT)
								--Dest.iIntValue;
							else
								--Dest.fFloatLiteral;

							break;
						}

						// Move the result to the destination

						Value value = ResolveOpPntr (0);
						value.SetValue (ref Dest);

						break;
					}

				// ---- String Processing

				case INSTR_CONCAT: {
						// Get a local copy of the destination operand (operand index 0)

						Value Dest = ResolveOpValue (0);

						// Get a local copy of the source string (operand index 1)

						string pstrSourceString = ResolveOpAsString (1);

						// If the destination isn't a string, do nothing

						if (Dest.iType != OP_TYPE_STRING)
							break;

						// Determine the length of the new string and allocate space for it (with a
						// null terminator)

						//int iNewStringLength = strlen (Dest.pstrStringLiteral) + strlen (pstrSourceString);
						//string pstrNewString = (string)malloc (iNewStringLength + 1);

						// Copy the old string to the new one

						//strcpy (pstrNewString, Dest.pstrStringLiteral);

						// Concatenate the destination with the source

						//strcat (pstrNewString, pstrSourceString);

						// Free the existing string in the destination structure and replace it
						// with the new string

						//free (Dest.pstrStringLiteral);
						Dest.pstrStringLiteral += pstrSourceString;

						// Copy the concatenated string pointer to its destination

						Value value = ResolveOpPntr (0);
						value.SetValue (ref Dest);


						break;
					}

				case INSTR_GETCHAR: {
						// Get a local copy of the destination operand (operand index 0)

						Value Dest = ResolveOpValue (0);

						// Get a local copy of the source string (operand index 1)

						string pstrSourceString = ResolveOpAsString (1);

						// Find out whether or not the destination is already a string

						Char pstrNewString;
						//if (Dest.iType == OP_TYPE_STRING) {
						//	// If it is, we can use it's existing string buffer as long as it's at
						//	// least 1 character

						//	if (strlen (Dest.pstrStringLiteral) >= 1) {
						//		pstrNewString = Dest.pstrStringLiteral;
						//	} else {
						//		free (Dest.pstrStringLiteral);
						//		pstrNewString = (string)malloc (2);
						//	}
						//} else {
						//	// Otherwise allocate a new string and set the type

						//	pstrNewString = (string)malloc (2);
						//	Dest.iType = OP_TYPE_STRING;
						//}

						// Get the index of the character (operand index 2)

						int iSourceIndex = ResolveOpAsInt (2);

						// Copy the character and append a null-terminator

						pstrNewString = pstrSourceString [iSourceIndex];
						//pstrNewString [1] = '\0';

						// Set the string pointer in the destination Value structure

						Dest.pstrStringLiteral = pstrNewString.ToString ();

						// Copy the concatenated string pointer to its destination

						Value value = ResolveOpPntr (0);
						value.SetValue (ref Dest);


						break;
					}

				case INSTR_SETCHAR: {
						// Get the destination index (operand index 1)

						int iDestIndex = ResolveOpAsInt (1);

						// If the destination isn't a string, do nothing

						if (ResolveOpType (0) != OP_TYPE_STRING)
							break;

						// Get the source character (operand index 2)

						string pstrSourceString = ResolveOpAsString (2);

						// Set the specified character in the destination (operand index 0)

						Value value = ResolveOpPntr (0);
						value.pstrStringLiteral.Remove (iDestIndex).Insert (iDestIndex, pstrSourceString);

						break;
					}

				// ---- Conditional Branching

				case INSTR_JMP: {
						// Get the index of the target instruction (opcode index 0)

						int iTargetIndex = ResolveOpAsInstrIndex (0);

						// Move the instruction pointer to the target

						g_Scripts [g_iCurrThread].InstrStream.iCurrInstr = iTargetIndex;

						break;
					}

				case INSTR_JE:
				case INSTR_JNE:
				case INSTR_JG:
				case INSTR_JL:
				case INSTR_JGE:
				case INSTR_JLE: {
						// Get the two operands

						Value Op0 = ResolveOpValue (0);
						Value Op1 = ResolveOpValue (1);

						// Get the index of the target instruction (opcode index 2)

						int iTargetIndex = ResolveOpAsInstrIndex (2);

						// Perform the specified comparison and jump if it evaluates to true

						bool iJump = false;

						switch (iOpcode) {
						// Jump if Equal

						case INSTR_JE: {
								switch (Op0.iType) {
								case OP_TYPE_INT:
									if (Op0.iIntValue == Op1.iIntValue)
										iJump = true;
									break;

								case OP_TYPE_FLOAT:
									if (Op0.fFloatLiteral == Op1.fFloatLiteral)
										iJump = true;
									break;

								case OP_TYPE_STRING:
									if (String.Compare (Op0.pstrStringLiteral, Op1.pstrStringLiteral) == 0)
										iJump = true;
									break;
								}
								break;
							}

						// Jump if Not Equal

						case INSTR_JNE: {
								switch (Op0.iType) {
								case OP_TYPE_INT:
									if (Op0.iIntValue != Op1.iIntValue)
										iJump = true;
									break;

								case OP_TYPE_FLOAT:
									if (Op0.fFloatLiteral != Op1.fFloatLiteral)
										iJump = true;
									break;

								case OP_TYPE_STRING:
									if (String.Compare (Op0.pstrStringLiteral, Op1.pstrStringLiteral) != 0)
										iJump = true;
									break;
								}
								break;
							}

						// Jump if Greater

						case INSTR_JG:

							if (Op0.iType == OP_TYPE_INT) {
								if (Op0.iIntValue > Op1.iIntValue)
									iJump = true;
							} else {
								if (Op0.fFloatLiteral > Op1.fFloatLiteral)
									iJump = true;
							}

							break;

						// Jump if Less

						case INSTR_JL:

							if (Op0.iType == OP_TYPE_INT) {
								if (Op0.iIntValue < Op1.iIntValue)
									iJump = true;
							} else {
								if (Op0.fFloatLiteral < Op1.fFloatLiteral)
									iJump = true;
							}

							break;

						// Jump if Greater or Equal

						case INSTR_JGE:

							if (Op0.iType == OP_TYPE_INT) {
								if (Op0.iIntValue >= Op1.iIntValue)
									iJump = true;
							} else {
								if (Op0.fFloatLiteral >= Op1.fFloatLiteral)
									iJump = true;
							}

							break;

						// Jump if Less or Equal

						case INSTR_JLE:

							if (Op0.iType == OP_TYPE_INT) {
								if (Op0.iIntValue <= Op1.iIntValue)
									iJump = true;
							} else {
								if (Op0.fFloatLiteral <= Op1.fFloatLiteral)
									iJump = true;
							}

							break;
						}

						// If the comparison evaluated to true, make the jump

						if (iJump)
							g_Scripts [g_iCurrThread].InstrStream.iCurrInstr = iTargetIndex;

						break;
					}

				// ---- The Stack Interface

				case INSTR_PUSH: {
						// Get a local copy of the source operand (operand index 0)

						Value Source = ResolveOpValue (0);

						// Push the value onto the stack

						Push (g_iCurrThread, Source);

						break;
					}

				case INSTR_POP: {
						// Pop the top of the stack into the destination

						Value value = ResolveOpPntr (0);
						value = Pop (g_iCurrThread);

						break;
					}

				// ---- The Function Call Interface

				case INSTR_CALL: {
						// Get a local copy of the function index

						int iFuncIndex = ResolveOpAsFuncIndex (0);

						// Advance the instruction pointer so it points to the instruction
						// immediately following the call

						++g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

						// Call the function

						CallFunc (g_iCurrThread, iFuncIndex);

						break;
					}

				case INSTR_RET: {
						// Get the current function index off the top of the stack and use it to get
						// the corresponding function structure

						Value FuncIndex = Pop (g_iCurrThread);

						// Check for the presence of a stack base marker

						if (FuncIndex.iType == OP_TYPE_STACK_BASE_MARKER)
							iExitExecLoop = true;

						// Get the previous function index

						Func CurrFunc = GetFunc (g_iCurrThread, FuncIndex.iIntValue);
						int iFrameIndex = FuncIndex.iOffsetIndex;

						// Read the return address structure from the stack, which is stored one
						// index below the local data

						Value ReturnAddr = GetStackValue (g_iCurrThread, g_Scripts [g_iCurrThread].Stack.iTopIndex - (CurrFunc.iLocalDataSize + 1));

						// Pop the stack frame along with the return address

						PopFrame (CurrFunc.iStackFrameSize);

						// Restore the previous frame index

						g_Scripts [g_iCurrThread].Stack.iFrameIndex = iFrameIndex;

						// Make the jump to the return address

						g_Scripts [g_iCurrThread].InstrStream.iCurrInstr = ReturnAddr.iIntValue;

						break;
					}

				case INSTR_CALLHOST: {
						// Use operand zero to index into the host API call table and get the
						// host API function name

						Value HostAPICall = ResolveOpValue (0);
						int iHostAPICallIndex = HostAPICall.iHostAPICallIndex;

						// Get the name of the host API function

						string pstrFuncName = GetHostAPICall (iHostAPICallIndex);

						// Search through the host API until the matching function is found

						bool iMatchFound = false;
						for (int iHostAPIFuncIndex = 0; iHostAPIFuncIndex < MAX_HOST_API_SIZE; ++iHostAPIFuncIndex) {
							// Get a pointer to the name of the current host API function

							string pstrCurrHostAPIFunc = g_HostAPI [iHostAPIFuncIndex].pstrName;

							// If it equals the requested name, it might be a match

							if (String.Compare (pstrFuncName, pstrCurrHostAPIFunc) == 0) {
								// Make sure the function is visible to the current thread

								int iThreadIndex = g_HostAPI [iHostAPIFuncIndex].iThreadIndex;
								if (iThreadIndex == g_iCurrThread || iThreadIndex == XS_GLOBAL_FUNC) {
									iMatchFound = true;
									break;
								}
							}
						}

						// If a match was found, call the host API funcfion and pass the current
						// thread index

						if (iMatchFound)
							g_HostAPI [iHostAPICallIndex].fnFunc (g_iCurrThread);

						break;
					}

				// ---- Misc

				case INSTR_PAUSE: {
						// Get the pause duration

						int iPauseDuration = ResolveOpAsInt (0);

						// Determine the ending pause time

						g_Scripts [g_iCurrThread].iPauseEndTime = iCurrTime + iPauseDuration;

						// Pause the script

						g_Scripts [g_iCurrThread].iIsPaused = true;

						break;
					}

				case INSTR_EXIT: {
						// Resolve operand zero to find the exit code

						Value ExitCode = ResolveOpValue (0);

						// Get it from the integer field

						int iExitCode = ExitCode.iIntValue;

						// Tell the XVM to stop executing the script

						g_Scripts [g_iCurrThread].iIsRunning = false;

						break;
					}
				}

				// If the instruction pointer hasn't been changed by an instruction, increment it

				if (iCurrInstr == g_Scripts [g_iCurrThread].InstrStream.iCurrInstr)
					++g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

				// If we aren't running indefinitely, check to see if the main timeslice has ended

				if (iTimesliceDur != XS_INFINITE_TIMESLICE)
					if (iCurrTime > iMainTimesliceStartTime + iTimesliceDur)
						break;

				// Exit the execution loop if the script has terminated

				if (iExitExecLoop)
					break;
			}
		}

		/******************************************************************************************
		*
		*	XS_StartScript ()
		*
		*   Starts the execution of a script.
		*/

		public void XS_StartScript (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// Set the thread's execution flag

			g_Scripts [iThreadIndex].iIsRunning = true;

			// Set the current thread to the script

			g_iCurrThread = iThreadIndex;

			// Set the activation time for the current thread to get things rolling

			g_iCurrThreadActiveTime = GetCurrTime ();
		}

		/******************************************************************************************
		*
		*	XS_StopScript ()
		*
		*   Stops the execution of a script.
		*/

		void XS_StopScript (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// Clear the thread's execution flag

			g_Scripts [iThreadIndex].iIsRunning = false;
		}

		/******************************************************************************************
		*
		*	XS_PauseScript ()
		*
		*   Pauses a script for a specified duration.
		*/

		void XS_PauseScript (int iThreadIndex, int iDur)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// Set the pause flag

			g_Scripts [iThreadIndex].iIsPaused = true;

			// Set the duration of the pause

			g_Scripts [iThreadIndex].iPauseEndTime = GetCurrTime () + iDur;
		}

		/******************************************************************************************
		*
		*	XS_UnpauseScript ()
		*
		*   Unpauses a script.
		*/

		void XS_UnpauseScript (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// Clear the pause flag

			g_Scripts [iThreadIndex].iIsPaused = false;
		}

		/******************************************************************************************
		*
		*	XS_GetReturnValueAsInt ()
		*
		*	Returns the last returned value as an integer.
		*/

		int XS_GetReturnValueAsInt (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return 0;

			// Return _RetVal's integer field

			return g_Scripts [iThreadIndex]._RetVal.iIntValue;
		}

		/******************************************************************************************
		*
		*	XS_GetReturnValueAsFloat ()
		*
		*	Returns the last returned value as an float.
		*/

		public float XS_GetReturnValueAsFloat (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return 0;

			// Return _RetVal's floating-point field

			return g_Scripts [iThreadIndex]._RetVal.fFloatLiteral;
		}

		/******************************************************************************************
		*
		*	XS_GetReturnValueAsString ()
		*
		*	Returns the last returned value as a string.
		*/

		public string XS_GetReturnValueAsString (int iThreadIndex)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return null;

			// Return _RetVal's string field

			return g_Scripts [iThreadIndex]._RetVal.pstrStringLiteral;
		}

		/******************************************************************************************
		*
		*   CopyValue ()
		*
		*   Copies a value structure to another, taking strings into account.
		*/

		void CopyValue (ref Value pDest, Value Source)
		{
			// If the destination already contains a string, make sure to free it first

			//if (pDest->iType == OP_TYPE_STRING)
			//free (pDest->pstrStringLiteral);

			// Copy the object

			pDest = Source;

			// Make a physical copy of the source string, if necessary

			//if (Source.iType == OP_TYPE_STRING) {
			//	pDest->pstrStringLiteral = (string)malloc (strlen (Source.pstrStringLiteral) + 1);
			//	strcpy (pDest->pstrStringLiteral, Source.pstrStringLiteral);
			//}
		}

		/******************************************************************************************
		*
		*   CoereceValueToInt ()
		*
		*   Coerces a Value structure from it's current type to an integer value.
		*/

		int CoerceValueToInt (Value Val)
		{
			// Determine which type the Value currently is

			switch (Val.iType) {
			// It's an integer, so return it as-is

			case OP_TYPE_INT:
				return Val.iIntValue;

			// It's a float, so cast it to an integer

			case OP_TYPE_FLOAT:
				return (int)Val.fFloatLiteral;

			// It's a string, so convert it to an integer

			case OP_TYPE_STRING:
				return Int32.Parse (Val.pstrStringLiteral);

			// Anything else is invalid

			default:
				return 0;
			}
		}

		/******************************************************************************************
		*
		*   CoereceValueToFloat ()
		*
		*   Coerces a Value structure from it's current type to an float value.
		*/

		float CoerceValueToFloat (Value Val)
		{
			// Determine which type the Value currently is

			switch (Val.iType) {
			// It's an integer, so cast it to a float

			case OP_TYPE_INT:
				return (float)Val.iIntValue;

			// It's a float, so return it as-is

			case OP_TYPE_FLOAT:
				return Val.fFloatLiteral;

			// It's a string, so convert it to an float

			case OP_TYPE_STRING:
				return Single.Parse (Val.pstrStringLiteral);

			// Anything else is invalid

			default:
				return 0;
			}
		}

		/******************************************************************************************
		*
		*   CoereceValueToString ()
		*
		*   Coerces a Value structure from it's current type to a string value.
		*/

		string CoerceValueToString (Value Val)
		{

			string pstrCoercion;
			//if (Val.iType != OP_TYPE_STRING)
			//pstrCoercion = (string)malloc (MAX_COERCION_STRING_SIZE + 1);

			// Determine which type the Value currently is

			switch (Val.iType) {
			// It's an integer, so convert it to a string

			case OP_TYPE_INT:
				//sprintf (pstrCoercion, "%f", Val.iIntLiteral);
				pstrCoercion = Val.iIntValue.ToString ();
				//                itoa ( Val.iIntLiteral, pstrCoercion, 10 );
				return pstrCoercion;

			// It's a float, so use sprintf () to convert it since there's no built-in function
			// for converting floats to strings

			case OP_TYPE_FLOAT:
				//sprintf (pstrCoercion, "%f", Val.fFloatLiteral);
				pstrCoercion = Val.fFloatLiteral.ToString ();

				return pstrCoercion;

			// It's a string, so return it as-is

			case OP_TYPE_STRING:
				return Val.pstrStringLiteral;

			// Anything else is invalid

			default:
				return null;
			}
		}

		/******************************************************************************************
		*
		*	GetOpType ()
		*
		*	Returns the type of the specified operand in the current instruction.
		*/

		int GetOpType (int iOpIndex)
		{
			// Get the current instruction

			int iCurrInstr = g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

			// Return the type

			return g_Scripts [g_iCurrThread].InstrStream.pInstrs [iCurrInstr].pOpList [iOpIndex].iType;
		}

		/******************************************************************************************
		*
		*   ResolveOpStackIndex ()
		*
		*   Resolves an operand's stack index, whether it's absolute or relative.
		*/

		int ResolveOpStackIndex (int iOpIndex)
		{
			// Get the current instruction

			int iCurrInstr = g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

			// Get the operand type type

			Value OpValue = g_Scripts [g_iCurrThread].InstrStream.pInstrs [iCurrInstr].pOpList [iOpIndex];

			// Resolve the stack index based on its type

			switch (OpValue.iType) {
			// It's an absolute index so return it as-is

			case OP_TYPE_ABS_STACK_INDEX:
				return OpValue.iIntValue;

			// It's a relative index so resolve it

			case OP_TYPE_REL_STACK_INDEX: {
					// First get the base index

					int iBaseIndex = OpValue.iIntValue;

					// Now get the index of the variable

					int iOffsetIndex = OpValue.iOffsetIndex;

					// Get the variable's value

					Value StackValue = GetStackValue (g_iCurrThread, iOffsetIndex);

					// Now add the variable's integer field to the base index to produce the
					// absolute index

					return iBaseIndex + StackValue.iIntValue;
				}

			// Return zero for everything else, but we shouldn't encounter this case

			default:
				return 0;
			}
		}

		/******************************************************************************************
		*
		*	ResolveOpValue ()
		*
		*	Resolves an operand and returns it's associated Value structure.
		*/

		Value ResolveOpValue (int iOpIndex)
		{
			// Get the current instruction

			int iCurrInstr = g_Scripts [g_iCurrThread].InstrStream.iCurrInstr;

			// Get the operand type

			Value OpValue = DeepCopy<Value> (g_Scripts [g_iCurrThread].InstrStream.pInstrs [iCurrInstr].pOpList [iOpIndex]);

			// Determine what to return based on the value's type

			switch (OpValue.iType) {
			// It's a stack index so resolve it

			case OP_TYPE_ABS_STACK_INDEX:
			case OP_TYPE_REL_STACK_INDEX: {
					// Resolve the index and use it to return the corresponding stack element

					int iAbsIndex = ResolveOpStackIndex (iOpIndex);
					return GetStackValue (g_iCurrThread, iAbsIndex);
				}

			// It's in _RetVal

			case OP_TYPE_REG:
				return DeepCopy<Value> (g_Scripts [g_iCurrThread]._RetVal);

			// Anything else can be returned as-is

			default:
				return OpValue;
			}
		}

		/******************************************************************************************
		*
		*	ResolveOpType ()
		*
		*	Resolves the type of the specified operand in the current instruction and returns the
		*	resolved type.
		*/

		int ResolveOpType (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Return the value type

			return OpValue.iType;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsInt ()
		*
		*	Resolves and coerces an operand's value to an integer value.
		*/

		int ResolveOpAsInt (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Coerce it to an int and return it

			int iInt = CoerceValueToInt (OpValue);
			return iInt;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsFloat ()
		*
		*	Resolves and coerces an operand's value to a floating-point value.
		*/

		float ResolveOpAsFloat (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Coerce it to a float and return it

			float fFloat = CoerceValueToFloat (OpValue);
			return fFloat;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsString ()
		*
		*	Resolves and coerces an operand's value to a string value, allocating the space for a
		*   new string if necessary.
		*/

		string ResolveOpAsString (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Coerce it to a string and return it

			string pstrString = CoerceValueToString (OpValue);
			return pstrString;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsInstrIndex ()
		*
		*	Resolves an operand as an intruction index.
		*/

		int ResolveOpAsInstrIndex (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Return it's instruction index

			return OpValue.iIntValue;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsFuncIndex ()
		*
		*	Resolves an operand as a function index.
		*/

		int ResolveOpAsFuncIndex (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Return the function index

			return OpValue.iIntValue;
		}

		/******************************************************************************************
		*
		*	ResolveOpAsHostAPICall ()
		*
		*	Resolves an operand as a host API call
		*/

		string ResolveOpAsHostAPICall (int iOpIndex)
		{
			// Resolve the operand's value

			Value OpValue = ResolveOpValue (iOpIndex);

			// Get the value's host API call index

			int iHostAPICallIndex = OpValue.iHostAPICallIndex;

			// Return the host API call

			return GetHostAPICall (iHostAPICallIndex);
		}

		/******************************************************************************************
		*
		*   ResolveOpPntr ()
		*
		*   Resolves an operand and returns a pointer to its Value structure.
		*/

		Value ResolveOpPntr (int iOpIndex)
		{
			// Get the method of indirection

			int iIndirMethod = GetOpType (iOpIndex);

			// Return a pointer to wherever the operand lies

			switch (iIndirMethod) {
			// It's on the stack

			case OP_TYPE_ABS_STACK_INDEX:
			case OP_TYPE_REL_STACK_INDEX: {
					int iStackIndex = ResolveOpStackIndex (iOpIndex);
					return g_Scripts [g_iCurrThread].Stack.pElmnts [ResolveStackIndex (iStackIndex)];
				}

			// It's _RetVal

			case OP_TYPE_REG:
				return g_Scripts [g_iCurrThread]._RetVal;
			}

			// Return null for anything else

			return null;
		}

		/******************************************************************************************
		*
		*	GetStackValue ()
		*
		*	Returns the specified stack value.
		*/

		Value GetStackValue (int iThreadIndex, int iIndex)
		{
			// Use ResolveStackIndex () to return the element at the specified index

			return DeepCopy<Value> (g_Scripts [iThreadIndex].Stack.pElmnts [ResolveStackIndex (iIndex)]);
		}

		/******************************************************************************************
		*
		*	SetStackValue ()
		*
		*	Sets the specified stack value.
		*/

		void SetStackValue (int iThreadIndex, int iIndex, Value Val)
		{
			// Use ResolveStackIndex () to set the element at the specified index

			g_Scripts [iThreadIndex].Stack.pElmnts [ResolveStackIndex (iIndex)] = Val;
		}

		/******************************************************************************************
		*
		*	Push ()
		*
		*	Pushes an element onto the stack.
		*/

		void Push (int iThreadIndex, Value Val)
		{
			// Get the current top element

			int iTopIndex = g_Scripts [iThreadIndex].Stack.iTopIndex;


			// Get the top value of stack
			//Value value =;

			// Put the value into the current top index

			CopyValue (ref g_Scripts [iThreadIndex].Stack.pElmnts [iTopIndex], Val);

			// Increment the top index

			++g_Scripts [iThreadIndex].Stack.iTopIndex;
		}

		/******************************************************************************************
		*
		*	Pop ()
		*
		*	Pops the element off the top of the stack.
		*/

		Value Pop (int iThreadIndex)
		{
			// Decrement the top index to clear the old element for overwriting

			--g_Scripts [iThreadIndex].Stack.iTopIndex;

			// Get the current top element

			int iTopIndex = g_Scripts [iThreadIndex].Stack.iTopIndex;

			// Use this index to read the top element

			Value Val = new Value ();
			CopyValue (ref Val, g_Scripts [iThreadIndex].Stack.pElmnts [iTopIndex]);

			// Return the value to the caller

			return Val;
		}

		/******************************************************************************************
		*
		*	PushFrame ()
		*
		*	Pushes a stack frame.
		*/

		void PushFrame (int iThreadIndex, int iSize)
		{
			// Increment the top index by the size of the frame

			g_Scripts [iThreadIndex].Stack.iTopIndex += iSize;

			// Move the frame index to the new top of the stack

			g_Scripts [iThreadIndex].Stack.iFrameIndex = g_Scripts [iThreadIndex].Stack.iTopIndex;
		}

		/******************************************************************************************
		*
		*	PopFrame ()
		*
		*	Pops a stack frame.
		*/

		void PopFrame (int iSize)
		{
			// Decrement the top index by the size of the frame

			g_Scripts [g_iCurrThread].Stack.iTopIndex -= iSize;

			// Move the frame index to the new top of the stack
		}

		/******************************************************************************************
		*
		*	GetFunc ()
		*
		*	Returns the function corresponding to the specified index.
		*/

		Func GetFunc (int iThreadIndex, int iIndex)
		{
			return g_Scripts [iThreadIndex].FuncTable.pFuncs [iIndex];
		}

		/******************************************************************************************
		*
		*	GetHostAPICall ()
		*
		*	Returns the host API call corresponding to the specified index.
		*/

		string GetHostAPICall (int iIndex)
		{
			return g_Scripts [g_iCurrThread].HostAPICallTable.ppstrCalls [iIndex];
		}

		/******************************************************************************************
		*
		*   GetCurrTime ()
		*
		*   Wrapper for the system-dependant method of determining the current time in
		*   milliseconds.
		*/

		int GetCurrTime ()
		{
			// This function is currently implemented with the WinAPI function GetTickCount ().
			// Change this line to make it compatible with other systems.

			//			int32_t currentTime;

			//# ifdef _WIN32
			//			currentTime = GetTickCount ();
			//#elif __APPLE__
			//        struct timeval current;
			//        gettimeofday (&current, null);
			//		currentTime = current.tv_sec* 1000 + current.tv_usec/1000;
			//#else
			//        return 0;
			//#endif
			return System.Environment.TickCount;
		}

		/******************************************************************************************
		*
		*   CallFunc ()
		*
		*   Calls a function based on its index.
		*/

		void CallFunc (int iThreadIndex, int iIndex)
		{
			Func DestFunc = GetFunc (iThreadIndex, iIndex);

			// Save the current stack frame index

			int iFrameIndex = g_Scripts [iThreadIndex].Stack.iFrameIndex;

			// Push the return address, which is the current instruction

			Value ReturnAddr = new Value ();
			ReturnAddr.iIntValue = g_Scripts [iThreadIndex].InstrStream.iCurrInstr;
			Push (iThreadIndex, ReturnAddr);

			// Push the stack frame + 1 (the extra space is for the function index
			// we'll put on the stack after it

			PushFrame (iThreadIndex, DestFunc.iLocalDataSize + 1);

			// Write the function index and old stack frame to the top of the stack

			Value FuncIndex = new Value ();
			FuncIndex.iIntValue = iIndex;
			FuncIndex.iOffsetIndex = iFrameIndex;
			SetStackValue (iThreadIndex, g_Scripts [iThreadIndex].Stack.iTopIndex - 1, FuncIndex);

			// Let the caller make the jump to the entry point

			g_Scripts [iThreadIndex].InstrStream.iCurrInstr = DestFunc.iEntryPoint;
		}

		/******************************************************************************************
		*
		*   XS_PassIntParam ()
		*
		*   Passes an integer parameter from the host to a script function.
		*/

		public void XS_PassIntParam (int iThreadIndex, int iInt)
		{
			// Create a Value structure to encapsulate the parameter

			Value Param = new Value ();
			Param.iType = OP_TYPE_INT;
			Param.iIntValue = iInt;

			// Push the parameter onto the stack

			Push (iThreadIndex, Param);
		}

		/******************************************************************************************
		*
		*   XS_PassFloatParam ()
		*
		*   Passes a floating-point parameter from the host to a script function.
		*/

		public void XS_PassFloatParam (int iThreadIndex, float fFloat)
		{
			// Create a Value structure to encapsulate the parameter

			Value Param = new Value ();
			Param.iType = OP_TYPE_FLOAT;
			Param.fFloatLiteral = fFloat;

			// Push the parameter onto the stack

			Push (iThreadIndex, Param);
		}

		/******************************************************************************************
		*
		*   XS_PassStringParam ()
		*
		*   Passes a string parameter from the host to a script function.
		*/

		public void XS_PassStringParam (int iThreadIndex, string pstrString)
		{
			// Create a Value structure to encapsulate the parameter

			Value Param = new Value ();
			Param.iType = OP_TYPE_STRING;
			//Param.pstrStringLiteral = (string)malloc (strlen (pstrString) + 1);
			//strcpy (Param.pstrStringLiteral, pstrString);
			Param.pstrStringLiteral = pstrString;
			// Push the parameter onto the stack

			Push (iThreadIndex, Param);
		}

		/******************************************************************************************
		*
		*   GetFuncIndexByName ()
		*
		*   Returns the index into the function table corresponding to the specified name.
		*/

		public int GetFuncIndexByName (int iThreadIndex, string pstrName)
		{
			// Loop through each function and look for a matching name

			for (int iFuncIndex = 0; iFuncIndex < g_Scripts [iThreadIndex].FuncTable.iSize; ++iFuncIndex) {
				// If the names match, return the index

				if (String.Compare (pstrName, g_Scripts [iThreadIndex].FuncTable.pFuncs [iFuncIndex].pstrName, true) == 0)
					return iFuncIndex;
			}

			// A match wasn't found, so return -1

			return -1;
		}

		/******************************************************************************************
		*
		*   XS_CallScriptFunc ()
		*
		*   Calls a script function from the host application.
		*/

		public void XS_CallScriptFunc (int iThreadIndex, string pstrName)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// ---- Calling the function

			// Preserve the current state of the VM

			int iPrevThreadMode = g_iCurrThreadMode;
			int iPrevThread = g_iCurrThread;

			// Set the threading mode for single-threaded execution

			g_iCurrThreadMode = THREAD_MODE_SINGLE;

			// Set the active thread to the one specified

			g_iCurrThread = iThreadIndex;

			// Get the function's index based on it's name

			int iFuncIndex = GetFuncIndexByName (iThreadIndex, pstrName);

			// Make sure the function name was valid

			if (iFuncIndex == -1)
				return;

			// Call the function

			CallFunc (iThreadIndex, iFuncIndex);

			// Set the stack base

			Value StackBase = GetStackValue (g_iCurrThread, g_Scripts [g_iCurrThread].Stack.iTopIndex - 1);
			StackBase.iType = OP_TYPE_STACK_BASE_MARKER;
			SetStackValue (g_iCurrThread, g_Scripts [g_iCurrThread].Stack.iTopIndex - 1, StackBase);

			// Allow the script code to execute uninterrupted until the function returns

			XS_RunScripts (XS_INFINITE_TIMESLICE);

			// ---- Handling the function return

			// Restore the VM state

			g_iCurrThreadMode = iPrevThreadMode;
			g_iCurrThread = iPrevThread;
		}

		/******************************************************************************************
		*
		*   XS_InvokeScriptFunc ()
		*
		*   Invokes a script function from the host application, meaning the call executes in sync
		*   with the script.
		*/

		public void XS_InvokeScriptFunc (int iThreadIndex, string pstrName)
		{
			// Make sure the thread index is valid and active

			if (!IsThreadActive (iThreadIndex))
				return;

			// Get the function's index based on its name

			int iFuncIndex = GetFuncIndexByName (iThreadIndex, pstrName);

			// Make sure the function name was valid

			if (iFuncIndex == -1)
				return;

			// Call the function

			CallFunc (iThreadIndex, iFuncIndex);
		}

		/******************************************************************************************
		*
		*   XS_RegisterHostAPIFunc ()
		*
		*   Registers a function with the host API.
		*/

		public void XS_RegisterHostAPIFunc (int iThreadIndex, string pstrName, HostAPIFuncPntr fnFunc)
		{
			// Loop through each function in the host API until a free index is found

			for (int iCurrHostAPIFunc = 0; iCurrHostAPIFunc < MAX_HOST_API_SIZE; ++iCurrHostAPIFunc) {
				// If the current index is free, use it

				if (!g_HostAPI [iCurrHostAPIFunc].iIsActive) {
					// Set the function's parameters

					g_HostAPI [iCurrHostAPIFunc].iThreadIndex = iThreadIndex;
					//g_HostAPI [iCurrHostAPIFunc].pstrName = (string)malloc (strlen (pstrName) + 1);
					//strcpy (g_HostAPI [iCurrHostAPIFunc].pstrName, pstrName);
					//strupr (g_HostAPI [iCurrHostAPIFunc].pstrName);
					g_HostAPI [iCurrHostAPIFunc].pstrName = pstrName.ToUpper ();
					g_HostAPI [iCurrHostAPIFunc].fnFunc = fnFunc;

					// Set the function to active

					g_HostAPI [iCurrHostAPIFunc].iIsActive = true;

					break;
				}
			}
		}

		/******************************************************************************************
		*
		*   XS_GetParamAsInt ()
		*
		*   Returns the specified integer parameter to a host API function.
		*/

		public int XS_GetParamAsInt (int iThreadIndex, int iParamIndex)
		{
			// Get the current top element

			int iTopIndex = g_Scripts [g_iCurrThread].Stack.iTopIndex;
			Value Param = g_Scripts [iThreadIndex].Stack.pElmnts [iTopIndex - (iParamIndex + 1)];

			// Coerce the top element of the stack to an integer

			int iInt = CoerceValueToInt (Param);

			// Return the value

			return iInt;
		}

		/******************************************************************************************
		*
		*   XS_GetParamAsFloat ()
		*
		*   Returns the specified floating-point parameter to a host API function.
		*/

		public float XS_GetParamAsFloat (int iThreadIndex, int iParamIndex)
		{
			// Get the current top element

			int iTopIndex = g_Scripts [g_iCurrThread].Stack.iTopIndex;
			Value Param = g_Scripts [iThreadIndex].Stack.pElmnts [iTopIndex - (iParamIndex + 1)];

			// Coerce the top element of the stack to a float

			float fFloat = CoerceValueToFloat (Param);

			// Return the value

			return fFloat;
		}

		/******************************************************************************************
		*
		*   XS_GetParamAsString ()
		*
		*   Returns the specified string parameter to a host API function.
		*/

		public string XS_GetParamAsString (int iThreadIndex, int iParamIndex)
		{
			// Get the current top element

			int iTopIndex = g_Scripts [g_iCurrThread].Stack.iTopIndex;
			Value Param = g_Scripts [iThreadIndex].Stack.pElmnts [iTopIndex - (iParamIndex + 1)];

			// Coerce the top element of the stack to a string

			string pstrString = CoerceValueToString (Param);

			// Return the value

			return pstrString;
		}

		/******************************************************************************************
		*
		*   XS_ReturnFromHost ()
		*
		*   Returns from a host API function.
		*/

		public void XS_ReturnFromHost (int iThreadIndex, int iParamCount)
		{
			// Clear the parameters off the stack

			g_Scripts [iThreadIndex].Stack.iTopIndex -= iParamCount;
		}

		/******************************************************************************************
		*
		*   XS_ReturnIntFromHost ()
		*
		*   Returns an integer from a host API function.
		*/

		public void XS_ReturnIntFromHost (int iThreadIndex, int iParamCount, int iInt)
		{
			// Clear the parameters off the stack

			g_Scripts [iThreadIndex].Stack.iTopIndex -= iParamCount;

			// Put the return value and type in _RetVal

			g_Scripts [iThreadIndex]._RetVal.iType = OP_TYPE_INT;
			g_Scripts [iThreadIndex]._RetVal.iIntValue = iInt;
		}

		/******************************************************************************************
		*
		*   XS_ReturnFloatFromHost ()
		*
		*   Returns a float from a host API function.
		*/

		public void XS_ReturnFloatFromHost (int iThreadIndex, int iParamCount, float fFloat)
		{
			// Clear the parameters off the stack

			g_Scripts [iThreadIndex].Stack.iTopIndex -= iParamCount;

			// Put the return value and type in _RetVal

			g_Scripts [iThreadIndex]._RetVal.iType = OP_TYPE_FLOAT;
			g_Scripts [iThreadIndex]._RetVal.fFloatLiteral = fFloat;
		}

		/******************************************************************************************
		*
		*   XS_ReturnStringFromHost ()
		*
		*   Returns a string from a host API function.
		*/

		public void XS_ReturnStringFromHost (int iThreadIndex, int iParamCount, string pstrString)
		{
			// Clear the parameters off the stack

			g_Scripts [iThreadIndex].Stack.iTopIndex -= iParamCount;

			// Put the return value and type in _RetVal

			Value ReturnValue = new Value ();
			ReturnValue.iType = OP_TYPE_STRING;
			ReturnValue.pstrStringLiteral = pstrString;
			CopyValue (ref g_Scripts [iThreadIndex]._RetVal, ReturnValue);
		}

	}

}