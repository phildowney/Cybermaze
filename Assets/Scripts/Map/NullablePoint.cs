using System;

namespace Assets.Scripts {
    // CSharp won't let JsonUtility write Structs as null, so we need a class version.
    // However, it STILL writes an extra field called 'id' for some reason. Even overriding this with your own id field
    // does not work!
    // But when it unmarshalls that field, it turns it into null anyways, so... :S
    [Serializable]
    public class NullablePoint {
        public float x;
        public float y;

        public override string ToString() {
            return "{" + x + ", " + y + "}";
        }
    }
}
