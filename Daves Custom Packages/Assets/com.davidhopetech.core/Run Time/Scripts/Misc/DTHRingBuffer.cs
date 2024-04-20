using System;

public class DTHRingBuffer<T>
{
    private  T[] _circularBuffer;
    readonly int _size;
    private  int _headCurrsor = 0;
    private  int _tailCurrsor = 0;
 
    public DTHRingBuffer(int size)
    {
        _size          = size;
        _circularBuffer = new T[size];
    }

    public void Add(T obj)
    {
        _circularBuffer[_headCurrsor] = obj;
        _headCurrsor                  = (_headCurrsor + 1) % _size;
        if (_headCurrsor == _tailCurrsor)
        {
            _tailCurrsor = (_tailCurrsor + 1) % _size;
        }
    }

    public T Read()
    {
        if (_tailCurrsor == _headCurrsor)
            throw new Exception("Tried to read from empty Circular Buffer");
        
        var val = _circularBuffer[_tailCurrsor];
        _tailCurrsor = (_tailCurrsor + 1) % _size;

        return val;
    }
 
    public T Peek()
    {
        return _circularBuffer[_headCurrsor];
    }
    
    public T this [int index] => _circularBuffer[(_headCurrsor + _size - index - 1) % _size];
}