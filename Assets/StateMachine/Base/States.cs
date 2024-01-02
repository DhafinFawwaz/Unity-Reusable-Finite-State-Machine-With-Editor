public class States<T, U> where T : Core<T, U> where U : States<T, U>
{
    T _core;
    protected T Core{get{return _core;} set{_core = value;}}
    public States(T contextCore)
    {
        _core = contextCore;
    }
}
