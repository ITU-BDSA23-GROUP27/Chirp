﻿namespace Chirp.CSVDBService.SimpleDB.Utility;

public abstract class Singleton<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get => instance ??= new T();
    }
}