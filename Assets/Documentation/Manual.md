Door Systems Guide
==================

This documentation describes how to use the `Door Systems` component in
your project.

Behaviours
----------

-   \[`DoorTriggerPressurePlate`\]
-   \[`KeyDoor`\]
-   \[`TriggerDoor`\]
-   \[`TriggerDoorKeyboard`\]
-   \[`UI_KeyHolder`\]

DoorTriggerPressurePlate
------------------------

This behaviour allows the user to use pressure plates to open and close
doors.

### Properties

-   `Door` Reference to the door parent object to get the animator
    component.
-   `timeToStayOpen` this float determines how long the door stays open
    after the user steps off of the pressure plate.

### Script

We create a reference to the door which has the `Animator` and
`DoorAnimation` script attached to it This script works by creating a
simple timer, once that timer equals 0 then the doors animator which is
referenced in the `SerializedField`is accessed in order close the door

    [SerializeField] private DoorAnimated door;
    private float timer;
    private void Update() {
           if (timer > 0f) {
               timer -= Time.deltaTime;
               if (timer <= 0f) {
                  door.CloseDoor();
               }
            }
       }

There are also two `OnTriggers` setup for if the player decides to step
on the pressure plate and either immediately get off or stay on it. When
the player steps onto the pressure plate then the `Animator` is accessed
to open the door, however if the player stays on the pressure plate then
the timer is constantly set to 2 so that the door will never close.
`private void OnTriggerStay(Collider collision) { float timeToStayOpen = 2f; timer = timeToStayOpen; } private void OnTriggerEnter(Collider collision) { door.OpenDoor();  } }`

KeyDoor
-------

This behaviour allows the user to destroy a door by having the correct
key equipped and being nearby to the assigned door.

### Properties

-   `Door` Reference to the door parent object to get the animator
    component.
-   `Key Type` - References the key required in order to open the door.
-   `Open Speed Curve` - Animation for the door opening
-   `Direction` - Determines which direction the door opens.
-   `Open Distance` - Float for how far the door opens.
-   `Open Speed Multiplier` - Float for how fast the door opens.
-   `Door Body` Reference to the door's model to open.

### Script

Unlike the pressure plates two objects do not need to access the same
door, therefore we can customise the doors animation a lot more to be
suitable for multiple game types, this allows us to control the
animations direction, speed and distance.

        [SerializeField] private DoorAnimated door;
        [SerializeField] private Key.KeyType keyType;
        public AnimationCurve openSpeedCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1, 0, 0), new Keyframe(0.8f, 1, 0, 0), new Keyframe(1, 0, 0, 0) }); //Contols the open speed at a specific time (ex. the door opens fast at the start then slows down at the end)
        public enum OpenDirection { x, y, z }
        public OpenDirection direction = OpenDirection.y;
        public float openDistance = 3f; //How far should door slide (change direction by entering either a positive or a negative value)
        public float openSpeedMultiplier = 2.0f; //Increasing this value will make the door open faster
        public Transform doorBody;
        bool open = false;
        Vector3 defaultDoorPosition;
        Vector3 currentDoorPosition;
        float openTime = 0;

In the start function we check that the doors body has been put into its
field, while also forcing the collider on the object to be set to true
which will be the area the player has to step into with the key

    void Start()
    {
        if (doorBody)
        {
            defaultDoorPosition = doorBody.localPosition;
        }
        GetComponent<Collider>().isTrigger = true;
    }

In the update function we include each of the directions in case the
user wants to use a different direction than the one used in the
package. When openTime is less than 1 then the door animation will play

       void Update()
       {
           if (!doorBody)
               return;
           if (openTime < 1)
           {
               openTime += Time.deltaTime * openSpeedMultiplier * openSpeedCurve.Evaluate(openTime);
           }
           if (direction == OpenDirection.x)
           {
               doorBody.localPosition = new Vector3(Mathf.Lerp(currentDoorPosition.x, defaultDoorPosition.x + (open ? openDistance : 0), openTime), doorBody.localPosition.y, doorBody.localPosition.z);
           }
           else if (direction == OpenDirection.y)
           {
               doorBody.localPosition = new Vector3(doorBody.localPosition.x, Mathf.Lerp(currentDoorPosition.y, defaultDoorPosition.y + (open ? openDistance : 0), openTime), doorBody.localPosition.z);
           }
           else if (direction == OpenDirection.z)
           {
               doorBody.localPosition = new Vector3(doorBody.localPosition.x, doorBody.localPosition.y, Mathf.Lerp(currentDoorPosition.z, defaultDoorPosition.z + (open ? openDistance : 0), openTime));
           }
       }

We also have a few voids that will reference to the `Key` script. In
order to open the door we set the openTime to 0, the user could also
just delete the door object instead, if the user choices to keep the
animations then the door will also close once the Player exits the
trigger that we set as true in the start function.

    public Key.KeyType GetKeyType()
     {
          return keyType;
     }
    public void OpenDoor()
     {
        // gameObject.SetActive(false);
        open = true;
        currentDoorPosition = doorBody.localPosition;
        openTime = 0;
    }
    private void OnTriggerExit(Collider other)
    {
        Invoke("CloseDoor", 2f);  
    }
    void CloseDoor()
    {
        open = false;
        currentDoorPosition = doorBody.localPosition;
        openTime = 0;
    }

TriggerDoor && TriggerDoorKeyboard
----------------------------------

These scripts allow the player to open the door by being nearby //
pressing a key on the keyboard.

### Properties

-   `Open Speed Curve` - Animation for the door opening
-   `Direction` - Determines which direction the door opens.
-   `Open Distance` - Float for how far the door opens.
-   `Open Speed Multiplier` - Float for how fast the door opens.
-   `Door Body` Reference to the door's model to open.

### Script

We use the same fields, void Start and Update from the `KeyDoor` script.
For the TriggerDoorKeyboard script, we use the InputManager to create a
new Input which we will use to open the door, the default key for this
is `K`, but can be changed.

For the TriggerDoor script, we do not need to include the
`&& Input.GetAxisRaw("OpenDoor") > 0` however we use OnTriggerEnter
instead as we want the door to open when we walk up to it immediately.

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetAxisRaw("OpenDoor") > 0)
            {
                open = true;
                currentDoorPosition = doorBody.localPosition;
                openTime = 0;
            }
        }

For both of the scripts we include this at the bottom of the script
which will close the door once the player leaves the doors trigger area.

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                open = false;
                currentDoorPosition = doorBody.localPosition;
                openTime = 0;
            }
        }
    }

UI\_KeyHolder
-------------

This behaviour allows the user to see the keys they currently have in
their inventory, once they open a door the key is removed from the UI.

### Properties

-   `keyHolder` Reference to the key holder script on the Player.

### Script

To start off with we are using Unitys Canvas therefore we need a
reference to the Unity UI.

    using UnityEngine.UI;

We then reference the `KeyHolder` script which is attached to the Player
prefab. Then we get the transform of an empty gameobject which is
parenting the keyTemplate transform

        [SerializeField] private KeyHolder keyHolder;
        private Transform container;
        private Transform keyTemplate;

When we launch the game, we find where those empty gameobjects are in
the hierarchy and disable the keytemplate so that no keys will show if
we forgot to remove them. Once we collect a key the system event will be
updated and will then activate our custom void.

        private void Awake()
        {
            container = transform.Find("Container");
            keyTemplate = container.Find("KeyTemplate");
            keyTemplate.gameObject.SetActive(false);
        }
        void Start()
        {
            keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
        }
        void KeyHolder_OnKeysChanged(object sender, System.EventArgs e)
        {
            UpdateVisual();
        }

When we come into contact with a key object, we find out what
colour/object that key was using the keyholder list and then instantiate
a new keytemplate for that key, for this example we change the colour of
the key to match the colours in our list.

        void UpdateVisual()
        {
            foreach (Transform child in container) {
                if (child == keyTemplate) continue;
                Destroy(child.gameObject);
            }
            List<Key.KeyType> keyList = keyHolder.GetKeyList();
            container.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(keyList.Count - 1) * 80 / 2f, -220);
            for (int i = 0; i < keyList.Count; i++)
            {
                Key.KeyType keyType = keyList[i];
                Transform keyTransform = Instantiate(keyTemplate, container);
                keyTransform.gameObject.SetActive(true);
                keyTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(80 * i, 0);
                Image keyImage = keyTransform.Find("Image").GetComponent<Image>();
                switch (keyType)
                {
                    default:
                    case Key.KeyType.Red: keyImage.color = Color.red; break;
                    case Key.KeyType.Green: keyImage.color = Color.green; break;
                    case Key.KeyType.Blue: keyImage.color = Color.blue; break;
                    case Key.KeyType.Yellow: keyImage.color = Color.yellow; break;
                }
            } 
         }
    }
