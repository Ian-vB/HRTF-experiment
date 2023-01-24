import sys

def remove(filename):
    f = open(filename, 'r')
    string = f.read()
    chars = "(),"
    for char in chars:
        string = string.replace(char, "")
    print(string)
    f.close()
    output = open(filename, 'w')
    output.write(string)
    
    


if __name__ == '__main__':

    filename = sys.argv[1]
    remove(filename)