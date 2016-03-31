using System.Collections;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class SelectionController : MonoBehaviour
    {
        private bool _isEnlarging;
        [SerializeField] private IslandData _source;
        [SerializeField] private IslandData _target;

        public SelectionController(IslandData source)
        {
            _source = source;
        }

        private void Start()
        {
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit, 100.0f))
                return;

            // Debug.Log("You selected the " + hit.transform.parent.name);

            // island hit
            if (!hit.transform.parent.name.Equals("Island"))
                return;

            var currentPlayer = CurrentPlayer();
            var island = hit.transform.parent.parent;
            var islandData = island.GetComponent<IslandData>();
            var isOwnedByPlayer = islandData.PlayerUuid.Equals(currentPlayer.Uuid);
            // Debug.Log("You selected the [" + island.name + "] owned by local player? [" + isOwnedByPlayer + "]");

            // highlight selected island
            if (!_isEnlarging)
                StartCoroutine(Wiggle(hit.transform.parent, new Vector3(1.5f, 1.5f, 1.5f), .25f));

//            if (!isOwnedByPlayer)
//                return;

            if (_source == null)
            {
                _source = islandData;
            }
            else if (_target == null)
            {
                _target = islandData;
            }
            else
            {
                _source = null;
                _target = null;
            }

            if (_source == null || _target == null)
                return;

            // Debug.Log("send: " + _source.Uuid + " " + _target.Uuid);
            currentPlayer.CmdSendUnits(_source.Uuid, _target.Uuid);
            _source = null;
            _target = null;
        }

        private PlayerData CurrentPlayer()
        {
            return Registry.Instance.Player.FirstOrDefault(player => player.isLocalPlayer);
        }

        private IEnumerator Wiggle(Transform target, Vector3 toScale, float duration)
        {
            _isEnlarging = true;

            var elapsedTime = 0f;
            var startingScale = target.localScale;
            var currentScale = new Vector3();
            while (elapsedTime < duration)
            {
                currentScale.x = Mathf.Lerp(startingScale.x, toScale.x, Easing.Wiggle.easeIn(elapsedTime/duration/2));
                currentScale.y = 1.0f/currentScale.x;
                currentScale.z = 1.0f/currentScale.y;

                target.localScale = currentScale;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            toScale = startingScale;
            elapsedTime = 0f;
            currentScale = new Vector3();
            while (elapsedTime < duration)
            {
                currentScale.x = Mathf.Lerp(startingScale.x, toScale.x, Easing.Wiggle.easeIn(elapsedTime/duration/2));
                currentScale.y = 1.0f/currentScale.x;
                currentScale.z = 1.0f/currentScale.y;

                target.localScale = currentScale;

                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            target.localScale = startingScale;

            _isEnlarging = false;
        }
    }
}