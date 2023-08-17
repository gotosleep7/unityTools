using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;


namespace QFramework
{

    public abstract class BaseController : MonoBehaviour, IController
    {
        public IArchitecture GetArchitecture()
        {
            return BaseArchitecture.Interface;
        }
    }

    public abstract class BaseCommand : ICommand
    {
        public abstract void Execute();

        public IArchitecture GetArchitecture()
        {
            return BaseArchitecture.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {

        }
    }

    public abstract class BaseQuery<T> : IQuery<T>
    {
        public abstract T Do();


        public IArchitecture GetArchitecture()
        {
            return BaseArchitecture.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {

        }
    }

    public abstract class BaseSystem : ISystem
    {
        public virtual void Init()
        {

        }
        public IArchitecture GetArchitecture()
        {
            return BaseArchitecture.Interface;
        }

        public void SetArchitecture(IArchitecture architecture)
        {

        }
    }




}