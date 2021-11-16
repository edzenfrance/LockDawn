using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // Stage 1
    public static string S1_DoorKey_A = "Upper Floor";
    public static string S1_DoorKey_B = "Stock Room";
    public static string S1_DoorKey_C = "Small Room";
    public static string S1_DoorKey_D = "Large Room";
    public static string S1_DoorKey_E = "Bathroom";
    public static string S1_DoorKey_F = "Kitchen";

    public static string S1_DoorKey_A_Add = "<color=green>Door Key: " + S1_DoorKey_A;
    public static string S1_DoorKey_B_Add = "<color=green>Door Key: " + S1_DoorKey_B;
    public static string S1_DoorKey_C_Add = "<color=green>Door Key: " + S1_DoorKey_C;
    public static string S1_DoorKey_D_Add = "<color=green>Door Key: " + S1_DoorKey_D;
    public static string S1_DoorKey_E_Add = "<color=green>Door Key: " + S1_DoorKey_E;
    public static string S1_DoorKey_F_Add = "<color=green>Door Key: " + S1_DoorKey_F;

    public static string S1_KeyWarn_A = "This door requires <color=green>Key: " + S1_DoorKey_A + "</color> to open.\nThe key must be around here, find it!";
    public static string S1_KeyWarn_B = "This door requires <color=green>Key: " + S1_DoorKey_B + "</color> to open.\nThe key must be around here, find it!";
    public static string S1_KeyWarn_C = "This door requires <color=green>Key: " + S1_DoorKey_C + "</color> to open.\nThe key must be around here, find it!";
    public static string S1_KeyWarn_D = "This door requires <color=green>Key: " + S1_DoorKey_D + "</color> to open.\nThe key must be around here, find it!";
    public static string S1_KeyWarn_E = "This door requires <color=green>Key: " + S1_DoorKey_E + "</color> to open.\nThe key must be around here, find it!";
    public static string S1_KeyWarn_F = "This door requires <color=green>Key: " + S1_DoorKey_F + "</color> to open.\nThe key must be around here, find it!";

    public static string S1_Inventory_A = "<color=green>Door Key: " + S1_DoorKey_A + "</color> - Can be use to open a locked door.";
    public static string S1_Inventory_B = "<color=green>Door Key: " + S1_DoorKey_B + "</color> - Can be use to open a locked door.";
    public static string S1_Inventory_C = "<color=green>Door Key: " + S1_DoorKey_C + "</color> - Can be use to open a locked door.";
    public static string S1_Inventory_D = "<color=green>Door Key: " + S1_DoorKey_D + "</color> - Can be use to open a locked door.";
    public static string S1_Inventory_E = "<color=green>Door Key: " + S1_DoorKey_E + "</color> - Can be use to open a locked door.";
    public static string S1_Inventory_F = "<color=green>Door Key: " + S1_DoorKey_F + "</color> - Can be use to open a locked door.";

    public static string S1_ItemGet_A = "Get the <color=green>Door Key: " + S1_DoorKey_A;
    public static string S1_ItemGet_B = "Get the <color=green>Door Key: " + S1_DoorKey_B;
    public static string S1_ItemGet_C = "Get the <color=green>Door Key: " + S1_DoorKey_C;
    public static string S1_ItemGet_D = "Get the <color=green>Door Key: " + S1_DoorKey_D;
    public static string S1_ItemGet_E = "Get the <color=green>Door Key: " + S1_DoorKey_E;
    public static string S1_ItemGet_F = "Get the <color=green>Door Key: " + S1_DoorKey_F;

    // Stage 2
    public static string S2_DoorKey_A = "Lounge Room";
    public static string S2_DoorKey_B = "Storage Room";
    public static string S2_DoorKey_C = "Bathroom";
    public static string S2_DoorKey_D = "Large Room";
    public static string S2_DoorKey_E = "Bathroom";
    public static string S2_DoorKey_F = "Kitchen";

    public static string S2_DoorKey_A_Add = "<color=green>Door Key: " + S2_DoorKey_A;
    public static string S2_DoorKey_B_Add = "<color=green>Door Key: " + S2_DoorKey_B;
    public static string S2_DoorKey_C_Add = "<color=green>Door Key: " + S2_DoorKey_C;
    public static string S2_DoorKey_D_Add = "<color=green>Door Key: " + S2_DoorKey_D;
    public static string S2_DoorKey_E_Add = "<color=green>Door Key: " + S2_DoorKey_E;
    public static string S2_DoorKey_F_Add = "<color=green>Door Key: " + S2_DoorKey_F;

    public static string S2_KeyWarn_A = "This door requires <color=green>Key: " + S2_DoorKey_A + "</color> to open.\nThe key must be around here, find it!";
    public static string S2_KeyWarn_B = "This door requires <color=green>Key: " + S2_DoorKey_B + "</color> to open.\nThe key must be around here, find it!";
    public static string S2_KeyWarn_C = "This door requires <color=green>Key: " + S2_DoorKey_C + "</color> to open.\nThe key must be around here, find it!";
    public static string S2_KeyWarn_D = "This door requires <color=green>Key: " + S2_DoorKey_D + "</color> to open.\nThe key must be around here, find it!";
    public static string S2_KeyWarn_E = "This door requires <color=green>Key: " + S2_DoorKey_E + "</color> to open.\nThe key must be around here, find it!";
    public static string S2_KeyWarn_F = "This door requires <color=green>Key: " + S2_DoorKey_F + "</color> to open.\nThe key must be around here, find it!";

    public static string S2_Inventory_A = "<color=green>Door Key: " + S2_DoorKey_A + "</color> - Can be use to open a locked door.";
    public static string S2_Inventory_B = "<color=green>Door Key: " + S2_DoorKey_B + "</color> - Can be use to open a locked door.";
    public static string S2_Inventory_C = "<color=green>Door Key: " + S2_DoorKey_C + "</color> - Can be use to open a locked door.";
    public static string S2_Inventory_D = "<color=green>Door Key: " + S2_DoorKey_D + "</color> - Can be use to open a locked door.";
    public static string S2_Inventory_E = "<color=green>Door Key: " + S2_DoorKey_E + "</color> - Can be use to open a locked door.";
    public static string S2_Inventory_F = "<color=green>Door Key: " + S2_DoorKey_F + "</color> - Can be use to open a locked door.";

    public static string S2_ItemGet_A = "Get the <color=green>Door Key: " + S2_DoorKey_A;
    public static string S2_ItemGet_B = "Get the <color=green>Door Key: " + S2_DoorKey_B;
    public static string S2_ItemGet_C = "Get the <color=green>Door Key: " + S2_DoorKey_C;
    public static string S2_ItemGet_D = "Get the <color=green>Door Key: " + S2_DoorKey_D;
    public static string S2_ItemGet_E = "Get the <color=green>Door Key: " + S2_DoorKey_E;
    public static string S2_ItemGet_F = "Get the <color=green>Door Key: " + S2_DoorKey_F;

    // Misc Get
    public static string ItemGet_Vitamins = "Get the <color=green>vitamins</color>";
    public static string ItemGet_Alcohol = "Get the <color=green>alcohol</color>";
    public static string ItemGet_Facemask = "Get the <color=green>face mask</color>";
    public static string ItemGet_Faceshield = "Get the <color=green>face shield</color>";
    public static string ItemGet_Vaccine = "Get the <color=green>vaccine!</color>";
    public static string ItemGet_Coin = "Get the <color=yellow>Coin</color>";
    public static string ItemGet_Syrup = "Get the <color=green>Special Syrup</color>";

    // Main Item
    public static string gotVitamin = "You got the main item vitamins!\nExit the house to finish the stage!";
    public static string gotAlcohol = "You got the main item alcohol!\nExit the house to finish the stage!";
    public static string gotFaceMask = "You got the main item face mask!\nGoto the end of the road to finish the stage!";
    public static string gotFaceShield = "You got the main item face shield!\nExit the mall to finish the stage!";
    public static string gotVaccine = "You got the main item vitamins!\nExit the hospital to finish the stage!";

    public static string inventoryVitamin = "<color=green>Vitamins</color> - Main item that can increased your immunity.";
    public static string inventoryAlcohol = "<color=green>Alcohol</color> - Main item that can increased your immunity.";
    public static string inventoryFaceMask = "<color=green>FaceMask</color> - Main item that can increased your immunity.";
    public static string inventoryFaceShield = "<color=green>FaceShield</color> - Main item that can increased your immunity.";
    public static string inventoryVaccine = "<color=green>Vaccine</color> - Main item that can increased your immunity.";
    public static string inventorySyrup = "<color=green> Special Syrup</color> - Can be use to stop your health from draining.";
    public static string inventoryCoin = "<color=yellow>Coin</color> - Can be used to buy new skin in shop.";

    // Item Detection

    // Others
    public static string openingDoor = "Opening the door using <color=green>Key:";
    public static string addedToInventory = "added to inventory.";
    public static string coinAdded = "<color=yellow>+10 coins</color> " + addedToInventory;
    public static string stopInfection = "Special syrup stop the infection!";
    public static string notInfected = "You are not infected";

    public static string quarantineArea = "You are in quarantine area!\nWatch some information to learn about COVID-19 and to reduce your quarantine time.";
    public static string doorIsLocked = "The door doesnt budge.";

    // Survey
    public static string[][] surveyTexts = new string[][] {
      new string[] {
        "2",
        "B",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "While playing the game for the first time, what have you learned about the game that you can do in real life to avoid getting infected or spread the virus?",
        "A. Hide from infected",
        "B. Physical distancing",
      },
      new string[] {
        "3",
        "A",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "What is the most essential way to make your family safe from COVID-19 while inside your house?",
        "A. Use disinfectant regularly",
        "B. Wash hands using water only",
        "C. Use gloves inside the house everytime",
       },
       new string[] {
        "3",
        "B",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "When do you need to where your facemask?",
        "A. When sleeping",
        "B. When outside",
        "C. When an authority is looking",
       },
       new string[] {
        "3",
        "C",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "How do you wear your face shield properly?",
        "A. Over the head",
        "B. On the back of your head",
        "C. Aligned to your face",
      },
      new string[] {
        "2",
        "A",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "_______ is a biological preparation that provides active acquired immunity to a particular infectious disease.",
        "A. Vaccine",
        "B. Vitamin",
      },
      new string[] {
        "2",
        "A",
        "Physical distancing helps limit the spread of COVID-19 – this means we keep a distance of at least 1m from each other and avoid spending time in crowded places or in groups.",
        "You are now on the final stage of the game, does the game help you to understand how safety protocols work in a pandemic situation?",
        "A. Yes",
        "B. No",
      }
    };

    public static string surveyCorrect = "Your answer it correctly! You learn something!";
    public static string surveyWrong = "Your wrong! ";

    // Riddle
    public static string[][] riddleTexts = new string[][] {
      new string[] {
        "I am neither a guest or a trespasser be, to this place I belong, it belongs also to me.",
        "A. Door",
        "B. Home",
        "C. Mother",
        "D. Land Owner",
        "B"},
      new string[] {
        "What begins but has no end and is the ending of all that begins?",
        "A. Death",
        "B. Reborn",
        "C. War",
        "D. Justice",
        "A"},
      new string[] {
        "Only one color, but not one size, Stuck at the bottom, yet easily flies. Present in sun, but not in rain, Doing no harm, and feeling no pain. What is it?",
        "A. Sky",
        "B. Light",
        "C. Darkness",
        "D. Shadow",
        "D"},
      new string[] {
        "What begins but has no end and is the ending of all that begins?",
        "A. Death",
        "B. Reborn",
        "C. War",
        "D. Justice",
        "A"},
      new string[] {
        "What begins but has no end and is the ending of all that begins?",
        "A. Death",
        "B. Reborn",
        "C. War",
        "D. Justice",
        "A"}
    };

    public static string riddleCorrect = "Your riddle answer is correct! Congrats, you gained collectors item! You can view the item in collectors hall!";
    public static string riddleWrong = "Your riddle answer is wrong! Better luck next time!";

}
