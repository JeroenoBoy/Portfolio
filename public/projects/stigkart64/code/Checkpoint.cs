namespace Game.Scripts.Laps
{
    //  This class is simply a tag with some utils to make code shorter

    public class Checkpoint : MonoBehaviour
    {
        /// <summary>
        /// Get the current position
        /// </summary>
        public Vector3 position => transform.position;
        public Vector3 forward => transform.forward;
        public int index;


        //  More editor only code that could screw with performance

        private LapsManager m_Manager;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!m_Manager) m_Manager = GetComponentInParent<LapsManager>();
            if (m_Manager.Contains(this)) return;
            m_Manager.NodeAdded();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + forward);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, 0.5f);
        }
#endif

        private void Awake()
        {
            if (!m_Manager) m_Manager = GetComponentInParent<LapsManager>();
        }




        #region GETTERS

        // replicate behaviour of a linked-list
        public Checkpoint next => m_Manager.GetNode((index + 1) % m_Manager.totalNotes);
        public Checkpoint previous => m_Manager.GetNode(index - 1 < 0 ? index - 1 + m_Manager.totalNotes : index - 1);

        #endregion
    }
}