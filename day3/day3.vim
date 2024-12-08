function Sum(listOfNumbers)
	let sum = 0

	for [x, y] in a:listOfNumbers
		let sum += x * y
	endfor

	return sum
endfunction

function FindNumbersToMultiply(line)
	let pattern = '\vmul\((\d{1,3}),(\d{1,3})\)'
	let numbersFound = []
	let startIdx = 0

	while startIdx < len(a:line)
		let result = matchstrpos(a:line[startIdx:], pattern)
		" " echo 'Result: ' . string(result)

		if empty(result)
			break
		endif

		
		if result[0] != ''
			let numbers = matchlist(result[0], pattern)
			call add(numbersFound, [numbers[1], numbers[2]])
			let startIdx += result[1] + 1
		else
			break
		endif
	endwhile

	return numbersFound
endfunction

function Part1()
	execute 'normal! gg'

	let listOfNumbers = []

	" Let's use the buffer number 1 as the target file
	let lines = getbufline(1, 1, '$')

	for line in lines
		let foundNumbers = FindNumbersToMultiply(line)
		let listOfNumbers += foundNumbers
	endfor

	" " echo 'List of numbers: ' . string(listOfNumbers)
	" " echo 'Len:' . len(listOfNumbers)

	let sum = Sum(listOfNumbers)
	echo 'Sum: ' . string(sum)
endfunction

function ToggleMultiplier(currentState, funcName)
	if a:funcName == ''
		return a:currentState
	elseif a:funcName == 'do()'
		return 1
	else
		return 0
	endif
endfunction

function Part2()
	execute 'normal! gg'

	let listOfNumbers = []
	let pattern1 = '\v(do\(\)|don''t\(\))?(.{-})\ze(do|don''t)'
	let pattern2 = '\v(do\(\)|don''t\(\))?(.*)'
	let do = 1

	" Let's use the buffer number 1 as the target file
	let lines = getbufline(1, 1, '$')
	for line in lines
        	let startIdx = 0

		" echo 'starting'

		while startIdx < len(line)
			let result = matchstrpos(line[startIdx:], pattern1)

			" echo 'Search string: ' . string(line[startIdx:])
			" echo 'Result: ' . string(result)

			if empty(result)
				break
			endif

			
			if result[0] != ''
				let fullResult = matchlist(result[0], pattern2)
				" echo 'Full result:' . string(fullResult)
				let do = ToggleMultiplier(do, fullResult[1])
				if do ==  1
					let numbersFound = FindNumbersToMultiply(fullResult[2])
					let listOfNumbers += numbersFound
				endif
      				let startIdx += result[2]
    			else
				break
			endif
		endwhile

		" Get anything left at the end of the line
		if startIdx != (len(line) - 1)
			let result = matchstrpos(line[startIdx:], pattern2)

			" echo 'Search string: ' . string(line[startIdx:])
			" echo 'Result: ' . string(result)

			if result[0] != ''
				let fullResult = matchlist(result[0], pattern2)
				let do = ToggleMultiplier(do, fullResult[1])
				if do ==  1
					let numbersFound = FindNumbersToMultiply(fullResult[2])
					let listOfNumbers = listOfNumbers + numbersFound
				endif
			endif
		endif

	endfor

	echo 'List of numbers: ' . string(listOfNumbers)
	echo 'Len:' . len(listOfNumbers)

	let sum = Sum(listOfNumbers)

	echo 'Sum: ' . string(sum)
endfunction
