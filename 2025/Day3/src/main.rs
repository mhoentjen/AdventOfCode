use std::fs;

fn main() {
    let file_path = "input.txt";
    let banks = read_banks_from_file_part2(file_path);
    println!("Part 2 Result: {}", part2(banks));
}

fn read_banks_from_file_part1(file_path: &str) -> Vec<Vec<u16>> {
    let contents = fs::read_to_string(file_path)
        .expect("Should have been able to read the file");
    contents.lines().map(|l| l.chars().map(|c| c.to_digit(10).unwrap() as u16).collect::<Vec<u16>>()).collect()
}

fn read_banks_from_file_part2(file_path: &str) -> Vec<Vec<char>> {
    let contents = fs::read_to_string(file_path)
        .expect("Should have been able to read the file");
    contents.lines().map(|l| l.chars().collect::<Vec<char>>()).collect()
}

fn part1(banks: Vec<Vec<u16>>) -> u16 {
    let mut sum: u16 = 0;
    for bank in banks {
        let mut largest: u16 = 0;
        for first in 0..bank.len() {
            for second in first+1..bank.len() {
                let joltage = bank[first] * 10 + bank[second];
                if joltage > largest {
                    largest = joltage;
                }
            }
        }
        println!("Joltage: {}", largest);
        sum += largest
    }
    sum
}

fn part2(banks: Vec<Vec<char>>) -> u64 {
    let mut sum: u64 = 0;
    for bank in banks {
        let mut current_pos = 0;
        let mut value = 0u64;
        for digit in 0..12 {
            let end_pos = bank.len() - (11 - digit);
            let range = bank[current_pos..end_pos].iter().cloned();
            let largest_in_range = range.clone().max().unwrap();
            let found_pos;
            for (i, c) in range.into_iter().enumerate() {
                if c == largest_in_range {
                    found_pos = i;
                    value = value * 10 + c.to_digit(10).unwrap() as u64;
                    current_pos += found_pos + 1;
                    break;
                }
            }
        }

        println!("Joltage: {}", value);
        sum += value
    }
    sum
}