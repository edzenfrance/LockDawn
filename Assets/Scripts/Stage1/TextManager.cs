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

    public static string quarantineArea = "You are in quarantine area!\nWatch some information to learn about COVID-19 and to reduce your quarantine time.";
    public static string doorIsLocked = "The door doesnt budge.";

}
