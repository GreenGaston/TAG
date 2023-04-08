using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Can{
    void useCan();
    void OverDose();
    float getDosage();
    KindOfCan getKindOfCan();

    void UseCanPermanently();

    void UndoCan();
}
