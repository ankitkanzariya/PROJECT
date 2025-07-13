import cohere   #libraries for ai servicies
from rich import print     #enhances for terminal outputs
from dotenv import dotenv_values        # import dotenv to load enviroment variable from a .env file

# load inviroment variable from env file
env_vars = dotenv_values(".env")

#retrive api keys
CohereAPIKey = env_vars.get("CohereAPIKey")

#create a cohere clint using the providing keys
co = cohere.Client(api_key=CohereAPIKey)

#define a list of recognize keyword for catagarization
funcs = [
    "exit", "general", "realtime", "open", "close", "play", "generate image", "system", "content", "google serch", "youtube serch", "reminder"
]

# initilize an empty list to store message
messages = []

# define the preamble for ai model how catagories queries
preamble = """"""

# define a chat history with predefine user
ChatHistory = [
{"role": "User", "message": "how are you?"},
{"role": "Chatbot", "message": "general how are you?"},
{"role": "User", "message": "do you like pizza?"},
{"role": "Chatbot", "message": "general do you like pizza?"},
{"role": "User", "message": "open chrome and tell me about mahatma gandhi."},
{"role": "Chatbot", "message": "open chrome, general tell me about mahatma gandhi."},
{"role": "User", "message": "open chrome, open firefox"},
{"role": "Chatbot", "message": "open chrome, open firefox"},
{"role": "User", "message": "chat with me"},
{"role": "Chatbot", "message": "genereal chat with me"}
]

#define the main function for desicion making on queries
def FirstLayerDMM(prompt: str = "test"):
    messages.append({"role":})