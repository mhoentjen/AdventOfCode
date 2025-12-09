use std::fs;

fn main() {
    let file_path = "input.txt";
    let lines = read_lines_from_file(file_path);
    part2(lines.clone());
}

fn part1(lines: Vec<String>) {
    let mut position = 50;
    let mut zero_positions = 0;
    for line in lines {
        let instruction = read_instruction(line);
        position = execute_instruction(position, &instruction.0, instruction.1);
        println!("Position: {}", position);
        if position == 0 {
            zero_positions += 1;
        }
    }
    println!("Result: {zero_positions}");
}

fn part2(lines: Vec<String>) {
    let mut position = 50;
    let mut zero_positions = 0;
    for line in lines {
        let instruction = read_instruction(line);
        let (new_position, added_zero_positions) = simulate_movement(position, &instruction.0, instruction.1);
        position = new_position;
        println!("Position: {}", position);
        zero_positions += added_zero_positions;
    }
    println!("Result: {zero_positions}");
}

fn read_lines_from_file(file_path: &str) -> Vec<String> {
    let contents = fs::read_to_string(file_path)
        .expect("Should have been able to read the file");
    contents.lines().map(|line| line.to_string()).collect()
}

enum Direction {
    Left,
    Right,
}
fn read_instruction(line: String) -> (Direction, i32) {
    let direction_char = line.chars().next().unwrap();
    let distance_str = &line[1..];
    let distance: i32 = distance_str.parse().unwrap();
    let direction = match direction_char {
        'L' => Direction::Left,
        'R' => Direction::Right,
        _ => panic!("Invalid direction character"),
    };
    (direction, distance)
}

fn execute_instruction(position: i32, direction: &Direction, distance: i32) -> i32 {
    // Starting at the input position, move in the specified direction by the specified distance. Below 0, wrap around to 99. Above 99, wrap around to 0.
    match direction {
        Direction::Left => {
            println!("Moving left by {}", distance);
            let new_position = position - distance % 100;
            if new_position < 0 {
                100 + new_position
            } else {
                new_position
            }
        },
        Direction::Right => {
            println!("Moving right by {}", distance);
            let new_position = position + distance % 100;
            if new_position > 99 {
                new_position - 100
            } else {
                new_position
            }
        },
    }
}

fn simulate_movement(position: i32, direction: &Direction, distance: i32) -> (i32, i32) {
    let mut current_position = position;
    let mut zero_crossings = 0;
    for _ in 0..distance {
        current_position = match direction {
            Direction::Left => {
                if current_position == 0 {
                    99
                } else {
                    current_position - 1
                }
            },
            Direction::Right => {
                if current_position == 99 {
                    0
                } else {
                    current_position + 1
                }
            },
        };
        if current_position == 0 {
            zero_crossings += 1;
        }
        println!("Position: {}", current_position);
    }
    (current_position, zero_crossings)
}