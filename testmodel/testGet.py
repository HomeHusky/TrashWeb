data = {
        'Carboard': 0,
        'Dangerous': 0,
        'Glass': 0,
        'Metal': 0,
        'Other Garbage': 0,
        'Paper': 0,
        'Plastic': 0
    }

def get_values():
    list_temp = []
    for value in data.values():
        list_temp.append(value)
    return list_temp

def get_3_value(list_temp):
    valueRecycle = list_temp[0] + list_temp[2] + list_temp[3] + list_temp[5] + list_temp[6]
    valueDangerous = list_temp[1]
    valueOther = list_temp[4]
    newlist = [valueRecycle, valueDangerous, valueOther]
    return newlist

array = get_values()
newlist = get_3_value(array)
print(newlist)