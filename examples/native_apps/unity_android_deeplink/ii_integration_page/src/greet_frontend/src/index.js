import {AuthClient} from "@dfinity/auth-client"
import {SignIdentity} from "@dfinity/agent";
import {DelegationIdentity, Ed25519PublicKey } from "@dfinity/identity";

// A faked Ed25519KeyIdentity with only the public key provided.
class FakedEd25519KeyIdentity extends SignIdentity {
    constructor(publicKey) {
        super();
        this._publicKey = publicKey;
    }

    getPublicKey () {
        return this._publicKey;
    }
}

function fromHexString(hexString) {
    return new Uint8Array((hexString.match(/.{1,2}/g) ?? []).map(byte => parseInt(byte, 16))).buffer;
}

// Parse the public session key and instantiate a FakedEd25519KeyIdentity.
let fakedEd25519KeyIdentity;

var url = window.location.href;
var keyIndex = url.indexOf("sessionkey=");
if (keyIndex !== -1) {
    var sessionkey = url.substring(keyIndex + "sessionkey=".length);

    var publicKey = Ed25519PublicKey.fromDer(fromHexString(sessionkey));
    fakedEd25519KeyIdentity = new FakedEd25519KeyIdentity(publicKey);
}

let delegationIdentity;

const loginButton = document.getElementById("login");
loginButton.onclick = async (e) => {
    e.preventDefault();

    // Create an auth client.
    let authClient = await AuthClient.create({
        identity: fakedEd25519KeyIdentity,
    });

    // Start the login process and wait for it to finish.
    await new Promise((resolve) => {
        authClient.login({
            identityProvider: "https://identity.ic0.app/#authorize",
            onSuccess: resolve,
        });
    });

    // At this point we're authenticated, and we can get the identity from the auth client.
    const identity = authClient.getIdentity();
    if (identity instanceof DelegationIdentity) {
        delegationIdentity = identity;
    }

    return false;
};

const openButton = document.getElementById("open");
openButton.onclick = async (e) => {
    e.preventDefault();

    var url = "vincenttest1://hello?";
    if (delegationIdentity != null) {
        var delegationString = JSON.stringify(delegationIdentity.getDelegation().toJSON());
        console.log(delegationString);
        url = url + "delegation=" + encodeURIComponent(delegationString);
    }

    window.open(url, "_self");

    return false;
};
