import { signalR } from 'module';
const require = createRequire(import.signalR);

$(document).ready(function () {
    window.chat = createChatController();
    window.chat.loadUser();
});
const publicId = 12345678910;
function createChatController() {
    var user = {
        name: null,
        dtConnection: null,
        key: null
    };

    return {
        state: user,
        connection: null,
        loadUser: function () {
            this.state.name = prompt(
                "Write the nickname to enter the chat",
                "User23"
            );
            this.state.dtConnection = new Date();
            this.state.key = new Date().valueOf();
            this.connectUserToChat();
        },
        connectUserToChat: function () {
            startConnection(this);
        },
        sendMessage: function (to) {
            var chatMessage = {
                from: this.state,
                message: to.message,
                toId: to.destination
            };

            this.connection
                .invoke("SendMessage", chatMessage)
                .catch(err => console.log((x = err)));

            insertMessage(
                chatMessage.toId,
                "me",
                chatMessage.message,
                chatMessage.from.name
            );
            to.field.val("").focus();
        },
        onReceiveMessage: function () {
            this.connection.on("Receive", (sender, message) => {
                openChat(null, sender, message, false);
            });

            this.connection.on("Public", (sender, message) => {
                openChat(null, sender, message, true);
            });
        }
    };
}

async function startConnection(chat) {
    try {
        chat.connection = new require.HubConnectionBuilder()
            .withUrl("/chat?user=" + JSON.stringify(window.chat.state))
            .build();
        await chat.connection.start();

        //Carrega usuários no chat
        loadChat(chat.connection);

        chat.connection.onclose(async () => {
            await startConnection(chat);
        });

        chat.onReceiveMessage();
    } catch (err) {
        setTimeout(() => startConnection(chat.connection), 5000);
    }
}

async function loadChat(connection) {
    connection.on("chat", (users, user) => {
        const listUsers = data => {
            return users
                .map(u => {
                    if (u.key != window.chat.state.key)
                        return `
              <section class="user box_shadow_0" onclick="openChat(this)" data-id="${u.key
                            }" data-name="${u.name}">
              <span class="user_icon">${u.name.charAt(0)}</span>
              <p class="user_name"> ${u.name} </p>
              <span class="user_date"> ${new Date(
                                u.dtConnection
                            ).toLocaleDateString()}</span>
              </section>
              `;
                })
                .join("");
        };
        verifyOpenChats(users);
        $(".main")
            .empty()
            .append(listUsers);
    });
}
function verifyOpenChats(users) {
    document.querySelectorAll("section.chat").forEach(e => {
        var value = $(e).data("chat");
        if (!users.some(obj => obj.key == value) && $(e).data('chat') != publicId) $(e).remove();
    });
}

function openChat(e, sender, message, public) {
    var user = {
        id: e ? $(e).data("id") : sender.key,
        name: e ? $(e).data("name") : sender.name
    };
    if (!checkIfElementExist(public ? publicId : user.id, "chat")) {
        const chat = `
        <section class="chat" data-chat="${public ? publicId : user.id}">
        <header>
            ${public ? "Public" : user.name}
        </header>
        <main>
        </main>
        <footer>
            <input type="text" placeholder="Write your message" data-chat="${public ? publicId : user.id
            }">
            <a onclick="sendMessage(this)" data-chat="${public ? publicId : user.id
            }">Send</a>
        </footer>
        </section>
        `;

        $(".chats_wrapper").append(chat);
    }
    if (public && sender.key == window.chat.state.key) return;

    if (sender && message)
        insertMessage(
            public ? publicId : sender.key,
            "their",
            message,
            sender.name
        );
}

function insertMessage(target, who, message, name) {
    const chatMessage = `
    <div class="message ${who}"><span class="senderNameMessage">${name}</span>${message} <span>${new Date().toLocaleTimeString()}</span></div>
    `;
    $(`section[data-chat="${target}"]`)
        .find("main")
        .append(chatMessage);
}

function sendMessage(e) {
    var input = {
        destination: $(e).data("chat"),
        field: $(`input[data-chat="${$(e).data("chat")}"]`),
        message: $(`input[data-chat="${$(e).data("chat")}"]`).val()
    };

    window.chat.sendMessage(input);
}

function checkIfElementExist(id, data) {
    return (
        $("section[data-" + data + '="' + id + '"]') &&
        $("section[data-" + data + '="' + id + '"]').length > 0
    );
}
