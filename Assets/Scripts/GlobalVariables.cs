using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables
{
   static int gameSeed;

   public static void SetGameSeed(int seed)
   {
        gameSeed = seed;
   }

   public static int GetGameSeed()
   {
        return gameSeed;
   }
}
