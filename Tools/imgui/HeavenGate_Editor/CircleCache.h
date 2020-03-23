#pragma once

template<typename T, unsigned int capacity = 0>
class CircleCache
{
public:
    CircleCache();
    ~CircleCache();

    void PushBack(const T& elem);
    T PopBack();
    void PushBegin(const T& elem);
    T PopBegin();

private:
    void ExpandCapacity();

    static const unsigned int INIT_SIZE;
    T* m_cache;
    unsigned int m_head;
    unsigned int m_tail;
    unsigned int m_capacity;
};



template<typename T, unsigned int capacity /*= 0*/>
const unsigned int CircleCache<T, capacity>::INIT_SIZE = 0;

template<typename T, unsigned int capacity /*= 0*/>
CircleCache<T, capacity>::CircleCache()
{
    if (capacity == 0)
    {
        m_capacity = INIT_SIZE;

    }

    m_cache = new T[capacity];
    m_head = 0;
    m_tail = 0;
    m_capacity = capacity;
}



template<typename T, unsigned int capacity /*= 0*/>
CircleCache<T, capacity>::~CircleCache()
{
    if (m_cache != nullptr)
    {
        delete m_cache;
    }
    m_cache = nullptr;
}


template<typename T, unsigned int capacity /*= 0*/>
void CircleCache<T, capacity>::PushBack(const T& elem)
{

    //Is up to max capacity
    if (m_tail + 1 % m_capacity == m_head)
    {
        ExpandCapacity();
    }

    assert((m_tail + 1) % m_capacity != m_head);

    m_cache[m_tail] = elem;

    m_tail = (m_tail + 1) % m_capacity;
}


template<typename T, unsigned int capacity /*= 0*/>
T CircleCache<T, capacity>::PopBack()
{

    int pos = m_tail;
    pos--;

    if (pos < 0)
    {
        pos += m_capacity;
    }
    if (pos == m_head)
    {
        return nullptr;
    }

    const T& tmp = m_cache[m_tail];

    pos = m_tail;
    pos--;

    if (pos < 0)
    {
        m_tail += m_capacity -1;
    }
    else
    {
        m_tail--;
    }

    return tmp;
}


template<typename T, unsigned int capacity /*= 0*/>
void CircleCache<T, capacity>::ExpandCapacity()
{
    T* temp = m_cache;

    m_cache = new T[m_capacity * 2];

    for (int i = 0; i < m_tail; i++)
    {
        m_cache[i] = temp[i];
    }

    for (int i = m_capacity; i >= m_head; i--)
    {
        m_cache[m_capacity + i] = temp[i];
    }
    m_head += m_capacity;

    m_capacity *= 2;
}

template<typename T, unsigned int capacity /*= 0*/>
void CircleCache<T, capacity>::PushBegin(const T& elem)
{
    int pos = m_head ;
    pos--;
    if (pos < 0)
    {
        pos += m_capacity;
    }

    if (pos %  m_capacity == m_tail)
    {
        ExpandCapacity();
    }

    pos = m_head;
    pos--;
    if (pos < 0)
    {
        m_head += m_capacity - 1;
    }
    else
    {
        m_head--;
    }
    m_cache[m_head] = elem;

}

template<typename T, unsigned int capacity /*= 0*/>
T CircleCache<T, capacity>::PopBegin()
{
    if ( m_head  % m_capacity == m_tail)
    {
        return nullptr;
    }

    T tmp = m_cache[m_tail];
    m_head++;

    m_head = (m_head + 1) % m_capacity;

    return tmp;
}
